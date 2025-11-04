using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Identities;

[Serializable]
[IgnoreMultiTenancy]
public class ImportInvalidUsersCacheItem : IDownloadCacheItem
{
    public List<InvalidImportUsersFromFileDto> InvalidUsers { get; set; } = [];

    public ImportUsersFromFileType FileType { get; set; }

    public string Token { get; set; } = string.Empty;

    public Guid? TenantId { get; set; }
}