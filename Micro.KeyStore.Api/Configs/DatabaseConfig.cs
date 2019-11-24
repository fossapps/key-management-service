namespace Micro.KeyStore.Api.Configs
{
    public class DatabaseConfig
    {
        public bool AutoMigrate { set; get; }
        public string Host { set; get; }
        public int Port { set; get; }
        public string Name { set; get; }
        public string User { set; get; }
        public string Password { set; get; }
    }
}
