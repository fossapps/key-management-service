using System.Threading.Tasks;
using Micro.KeyStore.Api.Keys.Models;

namespace Micro.KeyStore.Api.Keys.Services
{
    public interface IKeyService
    {
        Task<Key> CreateKey(Key key);
    }
}
