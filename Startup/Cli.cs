using API;
using Business;
using GetWeed.Backend.Storage;
using Microsoft.EntityFrameworkCore;
using Startup.StartupExtensions;
using Storage;
using Storage.Cart;

namespace Startup;

public class Cli
{
    public static void StartServer(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHealthChecks();
        builder.Services.AddSingleton<ICartRepository, CartRepository>();
        builder.Services.AddSingleton<ICart, Cart>();
        builder.Services.AddDbContext<ApplicationContext>(ServiceLifetime.Singleton);
        builder.Services.AddAuthorization();
        builder.Services.SetupGraphqlServer();
        builder.Services.AddConfiguration(builder.Configuration);

        var app = builder.Build();
        app.UseHealthChecks("/livez");
        app.UseHealthChecks("/readyz");

        app.UseAuthorization();
        app.AddGraphQlEndpoints();

        app.Run();
    }

    public static void Migrate(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<ICartRepository, CartRepository>();
        builder.Services.AddSingleton<ICart, Cart>();
        builder.Services.AddDbContext<ApplicationContext>(ServiceLifetime.Singleton);
        builder.Services.AddAuthorization();
        builder.Services.SetupGraphqlServer();
        builder.Services.AddConfiguration(builder.Configuration);

        var app = builder.Build();
        app.Services.GetService<ApplicationContext>()!.Database.Migrate();
        Console.WriteLine("migrated");

    }
}
