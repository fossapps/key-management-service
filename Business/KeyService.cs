using Storage;

namespace Business;

public interface IKeyService
{
    Task<IEnumerable<Key>> FetchAllKeys();
    Task<Key> Create(string publicKey);
}

public class KeyService : IKeyService
{
    private readonly IKeyRepository _keyRepository;

    public KeyService(IKeyRepository keyRepository)
    {
        _keyRepository = keyRepository;
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
}
