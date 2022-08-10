using Microsoft.EntityFrameworkCore;

namespace Storage;

public interface IKeyRepository
{
    Task<IEnumerable<Key>> FindAllKeys();
    Task<Key> Create(Key key);
}

public class KeyRepository : IKeyRepository
{
    private readonly ApplicationContext _db;

    public KeyRepository(ApplicationContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Key>> FindAllKeys()
    {
        return await _db.Keys.AsNoTracking().Where(x => x.DeletedAt == null).ToListAsync();
    }

    public async Task<Key> Create(Key key)
    {
        var entity = await _db.Keys.AddAsync(key);
        await _db.SaveChangesAsync();
        return entity.Entity;
    }
}
