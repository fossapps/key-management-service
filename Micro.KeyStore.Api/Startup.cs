using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Micro.KeyStore.Api.Archive;
using Micro.KeyStore.Api.Archive.drivers;
using Micro.KeyStore.Api.Configs;
using Micro.KeyStore.Api.HealthCheck;
using Micro.KeyStore.Api.Keys.Repositories;
using Micro.KeyStore.Api.Keys.Services;
using Micro.KeyStore.Api.Models;
using Micro.KeyStore.Api.Uuid;
using Micro.KeyStore.Api.Workers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Slack;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Micro.KeyStore.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AddConfiguration(services, Configuration);
            services.AddMetrics();
            ConfigureDependencies(services, Configuration);
            ConfigureHealthChecks(services, Configuration);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescription => apiDescription.Last());
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Title",
                    Version = "v1"
                });
            });
            RegisterWorker(services);
        }

        private static void ConfigureHealthChecks(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddCheck<ConnectionToDbCheck>(nameof(ConnectionToDbCheck))
                .AddCheck<MemoryCheck>(nameof(MemoryCheck));
        }

        private static void ConfigureDependencies(IServiceCollection services, IConfiguration configuration)
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

        private static void AddConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseConfig>(configuration.GetSection("DatabaseConfig"));
            services.Configure<SlackLoggingConfig>(configuration.GetSection("Logging").GetSection("Slack"));
            services.Configure<ArchiveKeysConfig>(configuration.GetSection("ArchiveKeys"));
        }

        private static void RegisterWorker(IServiceCollection services)
        {
            services.AddHostedService<CleanupKeysWorker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IOptions<SlackLoggingConfig> slackConfig)
        {
            ConfigureSlack(loggerFactory, slackConfig.Value, env);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.RoutePrefix = "swagger";
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = WriteResponse,
                    AllowCachingResponses = false
                });
            });
        }
        private static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));
            return httpContext.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }

        private static void ConfigureSlack(ILoggerFactory loggerFactory, SlackLoggingConfig slackConfig, IHostEnvironment env)
        {
            if (string.IsNullOrEmpty(slackConfig.WebhookUrl))
            {
                return;
            }
            loggerFactory.AddSlack(new SlackConfiguration
            {
                MinLevel = slackConfig.MinLogLevel,
                WebhookUrl = new Uri(slackConfig.WebhookUrl)
            }, env.ApplicationName, env.EnvironmentName);
        }
    }
}
