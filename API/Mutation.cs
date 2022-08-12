using API.Attributes;
using API.Types;
using Business;
using Key = API.Types.Key;

namespace API;

public class Mutation
{
    [Protected("registerPublicKey")]
    public async Task<Key> RegisterPublicKey([Service] IKeyService keyService, string publicKey)
    {
        return (await keyService.Create(publicKey)).ToGraphQl();
    }
}
