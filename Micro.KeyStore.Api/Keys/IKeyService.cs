using System.Threading.Tasks;

namespace Micro.KeyStore.Api.Keys
{
    public interface IKeyService
    {
        Task<Key> CreateKey(Key key);
    }
}
