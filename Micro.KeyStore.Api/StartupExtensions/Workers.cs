using Micro.KeyStore.Api.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.KeyStore.Api.StartupExtensions
{
    public static class Workers
    {
        public static void RegisterWorkers(this IServiceCollection services)
        {
            services.AddHostedService<CleanupKeysWorker>();
        }
    }
}
