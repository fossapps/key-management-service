using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Storage.Cart;

namespace Storage;

public class ApplicationContext : DbContext
{
    private readonly DatabaseConfig _dbConfig;
    public DbSet<CartItem> CartItems { set; get; }

    public ApplicationContext(DbContextOptions options, IOptions<DatabaseConfig> dbOption) : base(options)
    {
        _dbConfig = dbOption.Value;
        Console.WriteLine(_dbConfig.Host);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connection = new NpgsqlConnectionStringBuilder
        {
            Host = _dbConfig.Host,
            Port = _dbConfig.Port,
            Database = _dbConfig.Database,
            Username = _dbConfig.User,
            Password = _dbConfig.Password,
            SslMode = SslMode.Disable
        };
        optionsBuilder
            .UseSnakeCaseNamingConvention()
            .UseNpgsql(connection.ConnectionString,
            options => { options.MigrationsAssembly("GetWeed.Backend.Storage"); });
    }
}
