namespace Business;

public record Key
{
    public string Id { set; get; }
    public string Body { set; get; }
}

internal static class KeyMapper
{
    internal static Key ToViewModel(this Storage.Key key)
    {
        return new Key
        {
            Body = key.Body,
            Id = key.Id
        };
    }
}
