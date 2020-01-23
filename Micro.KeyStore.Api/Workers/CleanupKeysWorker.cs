using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using Micro.KeyStore.Api.Archive;
using Micro.KeyStore.Api.Keys.Repositories;
using Micro.KeyStore.Api.Measurements;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

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
                    var keys = (await _keyRepository.FindCreatedBefore(
                            DateTime.Now.Subtract(TimeSpan.FromMinutes(_config.TimeToLiveInMinutes)),
                            _config.BatchSize))
                        .ToList();
                    foreach (var key in keys)
                    {
                        try
                        {
                            var elapsed = await Measurements.Timer.MeasureAsync(async () => await _archiver.Save(new Key
                            {
                                Body = key.Body,
                                Id = key.ShortSha
                            }));
                            WorkerMeasurements.MeasureTime(_metrics, elapsed);
                            WorkerMeasurements.MeasureArchiveOccurrence(_metrics);
                            await _keyRepository.Remove(key.Id);
                        }
                        catch (Exception e)
                        {
                            WorkerMeasurements.MeasureErrorOccurrence(_metrics);
                            _logger.LogError(e, "exception caught");
                        }

                        if (stoppingToken.IsCancellationRequested)
                        {
                            return;
                        }
                    }

                    _logger.LogDebug($"Archived {keys.Count} keys at {DateTime.Now}");
                    if (stoppingToken.IsCancellationRequested) return;
                    await Task.Delay(TimeSpan.FromSeconds(_config.BatchIntervalInSeconds), stoppingToken);
                }
            }
            catch (NpgsqlException)
            {
                _logger.LogWarning("db connection issues");
            }
            catch (OperationCanceledException)
            {
                // do nothing
            }
        }
    }
}
