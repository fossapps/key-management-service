using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Micro.KeyStore.Api.Keys.Models;

namespace Micro.KeyStore.Api.Keys.Repositories
{
    public interface IKeyRepository
    {
        Task<Key> FindById(string id);
        Task<Key> FindByShortSha(string shortSha);
        Task<Key> FindBySha(string sha);
        Task<IEnumerable<Key>> FindCreatedBefore(DateTime createdAfter);
        Task<IEnumerable<Key>> FindCreatedBefore(DateTime createdAfter, int numberOfItems);
        Task<string> FindNextShortSha(string sha);
        Task Remove(string id);
        Task<Key> Save(Key key);
    }
}
