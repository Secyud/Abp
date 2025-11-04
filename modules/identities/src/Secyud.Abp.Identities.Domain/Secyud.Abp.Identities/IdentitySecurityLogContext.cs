namespace Secyud.Abp.Identities;

public class IdentitySecurityLogContext
{
    public string? Identity { get; set; }

    public string? Action { get; set; }

    public string? UserName { get; set; }

    public string? ClientId { get; set; }

    public Dictionary<string, object> ExtraProperties { get; } = new();

    public virtual IdentitySecurityLogContext WithProperty(string key, object value)
    {
        ExtraProperties[key] = value;
        return this;
    }

}
