namespace Micro.KeyStore.Api.Archive
{
    public class ArchiveKeysConfig
    {
        public string Driver { set; get; }
        public string WebhookUrl { set; get; }
        public int BatchSize { set; get; } = 10;
        public int BatchIntervalInSeconds { set; get; } = 5;
        public int TimeToLiveInMinutes { set; get; } = 15;
    }
}
