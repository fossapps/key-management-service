using API.Configs;
using Storage;

namespace Startup.StartupExtensions;

public static class Configuration
{
    public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseConfig>(configuration.GetSection("DatabaseConfig"));
        services.Configure<Security>(configuration.GetSection("Security"));
    }
}
