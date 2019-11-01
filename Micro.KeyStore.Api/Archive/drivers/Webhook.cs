using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Micro.KeyStore.Api.Archive.drivers
{
    public class Webhook<T> : IDriver<T>
    {
        private readonly ArchiveKeysConfig _config;
        private readonly HttpClient _client;
        public Webhook(IOptions<ArchiveKeysConfig> config, HttpClient client)
        {
            _client = client;
            _config = config.Value;
        }

        public async Task Save(T item)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _config.WebhookUrl);
            var stringContent = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            request.Content = stringContent;
            var responseMessage = await _client.SendAsync(request, CancellationToken.None);
            responseMessage.EnsureSuccessStatusCode();
        }
    }
}