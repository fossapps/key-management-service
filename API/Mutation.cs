using Business;

namespace API;

public class Mutation
{
    public Task<Key> RegisterPublicKey([Service] IKeyService keyService, string publicKey)
    {
        return keyService.Create(publicKey);
    }
}
