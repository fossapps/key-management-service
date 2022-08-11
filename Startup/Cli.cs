using API;
using Business;
using Microsoft.EntityFrameworkCore;
using Startup.StartupExtensions;
using Storage;

namespace Startup;

public class Cli
{
    private static WebApplication GetWebHost(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDependencies();
        builder.Services.AddHealthChecks();
        builder.Services.AddAuthorization();
        builder.Services.SetupGraphqlServer();
        builder.Services.AddConfiguration(builder.Configuration);

        return builder.Build();
    }

    public static void StartServer(string[] args)
    {
        var app = GetWebHost(args);

        app.UseHealthChecks("/livez");
        app.UseHealthChecks("/readyz");

        app.UseAuthorization();
        app.AddGraphQlEndpoints();

        app.Run();
    }

    public static void CleanupKeys(string[] args, int hoursBefore)
    {
        GetWebHost(args)
            .Services
            .GetRequiredService<IKeyService>()
            .CleanupKeys(hoursBefore);
    }

    public static void Migrate(string[] args)
    {
        GetWebHost(args)
            .Services
            .GetRequiredService<ApplicationContext>()
            .Database
            .Migrate();
    }
}
