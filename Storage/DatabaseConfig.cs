namespace Storage;

public class DatabaseConfig
{
    public string Host { set; get; }
    public int Port { set; get; }
    public string Database { set; get; }
    public string User { set; get; }
    public string Password { set; get; }
    public string? MigrationTableName { set; get; }
}
