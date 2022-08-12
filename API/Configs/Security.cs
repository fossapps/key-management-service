namespace API.Configs;

public class Security
{
    public Dictionary<string, SecurityRequirements>? Rules { set; get; }
}

public class SecurityRequirements
{
    public string Header { set; get; }
    public string Value { set; get; }
}
