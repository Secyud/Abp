namespace Secyud.Abp.Identities.Session;

[Serializable]
public class IdentitySessionCacheItem
{
    public Guid Id { get; set; }

    public string SessionId { get; set; } = "";

    public string? IpAddress { get; set; }

    public DateTime? CacheLastAccessed { get; set; }

    public int HitCount { get; set; }
}