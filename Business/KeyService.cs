using HotChocolate.Execution;
using Storage;

namespace Business;

public interface IKeyService
{
    Task<Key> FindById(string id);
    Task<IEnumerable<Key>> FetchAllKeys();
    Task<Key> Create(string publicKey);
    Task CleanupKeys(int hoursBefore);
}

public class KeyService : IKeyService
{
    private readonly IKeyRepository _keyRepository;

    public KeyService(IKeyRepository keyRepository)
    {
        _keyRepository = keyRepository;
    }

    public async Task<Key> FindById(string id)
    {
        var key =  await _keyRepository.FindById(id);
        if (key == null)
        {
            throw new QueryException("key not found");
        }

        return key.ToViewModel();
    }

    public async Task<IEnumerable<Key>> FetchAllKeys()
    {
        return (await _keyRepository.FindAllKeys())
            .Select(x => x.ToViewModel());
    }

    public async Task<Key> Create(string publicKey)
    {
        var key = await _keyRepository.Create(new Storage.Key
        {
            Body = publicKey
        });
        return key.ToViewModel();
    }

    public Task CleanupKeys(int hoursBefore)
    {
        return _keyRepository.CleanupKeys(DateTime.Now.AddHours(-hoursBefore).ToUniversalTime());
    }
}
