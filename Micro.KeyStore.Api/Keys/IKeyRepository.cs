using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Micro.KeyStore.Api.Keys
{
    public interface IKeyRepository
    {
        Task<Key> FindById(string id);
        Task<Key> FindByShortSha(string shortSha);
        Task<Key> FindBySha(string sha);
        Task<IEnumerable<Key>> FindCreatedAfter(DateTime createdAfter);
        Task<string> FindNextShortSha(string sha);
        Task Remove(string id);
        Task<Key> Save(Key key);
    }
}
