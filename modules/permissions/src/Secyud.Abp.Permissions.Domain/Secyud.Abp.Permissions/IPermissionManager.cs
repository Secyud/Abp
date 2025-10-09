using Volo.Abp.Domain.Services;

namespace Secyud.Abp.Permissions;

public interface IPermissionManager : IDomainService
{
    Task<PermissionGrantInfo> GetAsync(string name,
        string providerName, string providerKey);

    Task<List<PermissionGrantInfo>> GetAsync(string[] names,
        string providerName, string providerKey);

    Task<List<PermissionGrantInfo>> GetListAsync(string? groupName,
        string providerName, string providerKey);

    Task<List<PermissionGroupInfo>> GetGroupsAsync();

    Task UpdateAsync(string providerName, string providerKey,
        string[] grantedPermissions, string[] deniedPermissions);

    Task DeleteAsync(string providerName, string providerKey);

    Task ClearCacheAsync(List<PermissionGrant> grants);
}