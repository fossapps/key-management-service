namespace Storage;

public class Key
{
    public string Id { set; get; }
    public string Body { set; get; }
    public DateTime CreatedAt { set; get; } = DateTime.Now.ToUniversalTime();
    public DateTime? DeletedAt { set; get; }
}
