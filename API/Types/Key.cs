using Business;

namespace API.Types;

public sealed record Key
{
    [Key] public string Id { set; get; }

    public string Body { set; get; }

    public static async Task<Key> GetKeyById([Service] IKeyService keyService, string Id)
    {
        return (await keyService.FindById(Id)).ToGraphQl();
    }
}

public static class BusinessKeyExtension
{
    public static Key ToGraphQl(this Business.Key key)
    {
        return new Key
        {
            Body = key.Body,
            Id = key.Id
        };
    }
}
