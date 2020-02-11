using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Micro.KeyStore.Api.Keys.Models;
using Micro.KeyStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Micro.KeyStore.Api.Keys.Repositories
{
    public class KeyRepository : IKeyRepository
    {
        private readonly ApplicationContext _db;

        public KeyRepository(ApplicationContext db)
        {
            _db = db;
        }

        public Task<Key> FindById(string id)
        {
            return _db.Keys.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Key> FindByShortSha(string shortSha)
        {
            return _db.Keys.AsNoTracking().FirstOrDefaultAsync(x => x.ShortSha == shortSha);
        }

        public Task<Key> FindBySha(string sha)
        {
            return _db.Keys.AsNoTracking().FirstOrDefaultAsync(x => x.Sha == sha);
        }

        public async Task<IEnumerable<Key>> FindCreatedBefore(DateTime createdBefore)
        {
            return await _db.Keys.AsNoTracking().Where(x => x.CreatedAt < createdBefore).ToListAsync();
        }

        public async Task<IEnumerable<Key>> FindCreatedBefore(DateTime createdBefore, int numberOfItems)
        {
            return await _db.Keys.AsNoTracking().Where(x => x.CreatedAt < createdBefore).Take(numberOfItems).ToListAsync();
        }

        public async Task<string> FindNextShortSha(string sha)
        {
            var info = await _db.Keys.AsNoTracking().Where(x => sha.StartsWith(x.ShortSha)).Select(x => new {x.ShortSha.Length}).OrderByDescending(x => x.Length).FirstOrDefaultAsync();
            var length = info?.Length ?? 0;
            return sha.Substring(0, length > 4 ? length : 4);
        }

        public async Task Remove(string id)
        {
            _db.Keys.Remove(new Key { Id = id });
            await _db.SaveChangesAsync();
        }

        public async Task<Key> Save(Key key)
        {
            var result = await _db.Keys.AddAsync(key);
            await _db.SaveChangesAsync();
            return result.Entity;
        }
    }
}