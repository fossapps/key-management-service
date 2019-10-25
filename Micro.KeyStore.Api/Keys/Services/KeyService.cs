using System.Threading.Tasks;
using App.Metrics;
using Micro.KeyStore.Api.Keys.Models;
using Micro.KeyStore.Api.Keys.Repositories;
using Micro.KeyStore.Api.Measurements;
using Micro.KeyStore.Api.Uuid;

namespace Micro.KeyStore.Api.Keys.Services
{
    public class KeyService : IKeyService
    {
        private readonly IKeyRepository _keyRepository;
        private readonly IUuidService _uuid;
        private readonly IMetrics _metrics;

        public KeyService(IKeyRepository keyRepository, IUuidService uuid, IMetrics metrics)
        {
            _keyRepository = keyRepository;
            _uuid = uuid;
            _metrics = metrics;
        }

        public async Task<Key> CreateKey(Key key)
        {
            var result = await _keyRepository.FindBySha(key.Sha);
            if (result != null)
            {
                throw new ConflictingKeyConflictException();
            }
            var (elapsed, shortSha) = await Timer.MeasureAsync(async () => await _keyRepository.FindNextShortSha(key.Sha));
            key.ShortSha = shortSha;
            ShortShaKey.Measure(_metrics, shortSha, elapsed);
            key.Id = _uuid.GenerateUuId();
            return await _keyRepository.Save(key);
        }
    }
}
