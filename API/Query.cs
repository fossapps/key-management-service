using API.types;
using Business;

namespace API;

public class Query
{
    public Task<User> Me()
    {
        return Task.FromResult(new User("user_id"));
    }

    public Task<IEnumerable<Key>> Keys([Service] IKeyService keyService)
    {
        return keyService.FetchAllKeys();
    }
}
