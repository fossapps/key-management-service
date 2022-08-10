namespace API.types;

public sealed record User(string Id)
{
    [Key] public string Id { get; set; } = Id;

    [ReferenceResolver]
    public static Task<User> GetUserByIdAsync(string id)
    {
        return Task.FromResult(new User(id));
    }
}
