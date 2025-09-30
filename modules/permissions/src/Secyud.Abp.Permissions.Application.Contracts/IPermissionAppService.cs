using JetBrains.Annotations;
using Volo.Abp.Application.Services;

namespace Secyud.Abp.Permissions;

public interface IPermissionAppService : IApplicationService
{
    Task<List<PermissionGrantInfoDto>> GetListAsync(string groupName,
        string providerName, string providerKey);

    Task<List<PermissionGroupInfoDto>> GetGroupsAsync();
    Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input);
}