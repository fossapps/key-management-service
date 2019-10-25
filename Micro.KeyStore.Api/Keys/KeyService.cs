using System.Threading.Tasks;
using Micro.KeyStore.Api.Uuid;

namespace Micro.KeyStore.Api.Keys
{
    public class KeyService : IKeyService
    {
        private readonly IKeyRepository _keyRepository;
        private readonly IUuidService _uuid;

        public KeyService(IKeyRepository keyRepository, IUuidService uuid)
        {
            _keyRepository = keyRepository;
            _uuid = uuid;
        }

        public async Task<Key> CreateKey(Key key)
        {
            key.ShortSha = await _keyRepository.FindNextShortSha(key.Sha);
            key.Id = _uuid.GenerateUuId();
            return await _keyRepository.Save(key);
        }
    }
}
