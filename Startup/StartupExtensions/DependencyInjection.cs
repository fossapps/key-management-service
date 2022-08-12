using Business;
using Storage;

namespace Startup.StartupExtensions;

public static class DependencyInjection
{
    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IKeyRepository, KeyRepository>();
        services.AddSingleton<IKeyService, KeyService>();
        services.AddDbContext<ApplicationContext>(ServiceLifetime.Singleton);
        services.AddHttpContextAccessor();
    }
}
