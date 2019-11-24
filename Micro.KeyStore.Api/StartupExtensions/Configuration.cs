using Micro.KeyStore.Api.Archive;
using Micro.KeyStore.Api.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.KeyStore.Api.StartupExtensions
{
    public static class Configuration
    {
        public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseConfig>(configuration.GetSection("DatabaseConfig"));
            services.Configure<SlackLoggingConfig>(configuration.GetSection("Logging").GetSection("Slack"));
            services.Configure<ArchiveKeysConfig>(configuration.GetSection("ArchiveKeys"));
        }
    }
}
