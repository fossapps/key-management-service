using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using Micro.KeyStore.Api.Archive;
using Micro.KeyStore.Api.Keys.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Micro.KeyStore.Api.Workers
{
    public class CleanupKeysWorker : BackgroundService
    {
        private readonly ILogger<CleanupKeysWorker> _logger;
        private readonly IKeyRepository _keyRepository;
        private readonly IDriver<Key> _archiver;
        private readonly IMetrics _metrics;
        private readonly ArchiveKeysConfig _config;

        public CleanupKeysWorker(ILogger<CleanupKeysWorker> logger, IServiceScopeFactory serviceScopeFactory, IDriver<Key> archiver, IOptions<ArchiveKeysConfig> config, IMetrics metrics)
        {
            _logger = logger;
            _archiver = archiver;
            _metrics = metrics;
            _config = config.Value;
            _keyRepository = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IKeyRepository>();
        }

        public override void Dispose()
        {
            base.Dispose();
            _logger.LogWarning("stopped");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var keys = await _keyRepository.FindCreatedBefore(
                        DateTime.Now.Subtract(TimeSpan.FromMinutes(_config.TimeToLiveInMinutes)), _config.BatchSize);
                    foreach (var key in keys)
                    {
                        try
                        {
                            await _archiver.Save(new Key
                            {
                                Body = key.Body,
                                Id = key.ShortSha
                            });
                            await _keyRepository.Remove(key.Id);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, "exception caught");
                        }
                    }

                    _logger.LogInformation($"Archived {keys.Count()} keys at {DateTime.Now}");
                    if (stoppingToken.IsCancellationRequested) return;
                    await Task.Delay(TimeSpan.FromSeconds(_config.BatchIntervalInSeconds), stoppingToken);
                }
            }
            catch (OperationCanceledException e)
            {
                // do nothing
            }
        }
    }
}
