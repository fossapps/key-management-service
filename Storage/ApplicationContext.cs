using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Storage.Generators;

namespace Storage;

public class ApplicationContext : DbContext
{
    private readonly DatabaseConfig _dbConfig;

    public ApplicationContext(DbContextOptions options, IOptions<DatabaseConfig> dbOption) : base(options)
    {
        _dbConfig = dbOption.Value;
    }

    public DbSet<Key> Keys { set; get; }

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
                options =>
                {
                    options.MigrationsAssembly("Storage");
                    if (_dbConfig.MigrationTableName != null)
                        options.MigrationsHistoryTable(_dbConfig.MigrationTableName);
                });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Key>().Property(x => x.Id).ValueGeneratedOnAdd().HasValueGenerator<IdValueGenerator>();

        base.OnModelCreating(modelBuilder);
    }
}
