namespace Secyud.Abp.Identities;

public interface IDownloadCacheItem
{
    public string Token { get; set; }

    public Guid? TenantId { get; set; }
}