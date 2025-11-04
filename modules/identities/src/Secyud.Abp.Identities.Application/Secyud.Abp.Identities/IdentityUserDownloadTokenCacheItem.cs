using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Identities;

[Serializable]
[IgnoreMultiTenancy]
public class IdentityUserDownloadTokenCacheItem : IDownloadCacheItem
{
    public string Token { get; set; } = string.Empty;

    public Guid? TenantId { get; set; }
}