using System;
using System.Net.Http;
using Micro.KeyStore.Api.Archive;
using Micro.KeyStore.Api.Archive.drivers;
using Micro.KeyStore.Api.Keys.Repositories;
using Micro.KeyStore.Api.Keys.Services;
using Micro.KeyStore.Api.Models;
using Micro.KeyStore.Api.Uuid;
using Micro.KeyStore.Api.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.KeyStore.Api.StartupExtensions
{
    public static class DependencyInjection
    {
        public static void ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>();
            services.AddSingleton<IUuidService, UuidService>();
            services.AddScoped<IKeyRepository, KeyRepository>();
            services.AddScoped<IKeyService, KeyService>();
            var client = new HttpClient {Timeout = TimeSpan.FromSeconds(5)};
            services.AddSingleton(client);
            var driver = configuration.GetSection("ArchiveKeys").Get<ArchiveKeysConfig>().Driver;
            ConfigureArchiveDriver(driver, services);
        }

        private static void ConfigureArchiveDriver(string driver, IServiceCollection services)
        {
            switch (driver)
            {
                case "noop":
                    services.AddSingleton<IDriver<Key>, Noop<Key>>();
                    break;
                case "webhook":
                    services.AddSingleton<IDriver<Key>, Webhook<Key>>();
                    break;
                default:
                    throw new ArgumentException($"driver ${driver} not found");
            }
        }
    }
}
