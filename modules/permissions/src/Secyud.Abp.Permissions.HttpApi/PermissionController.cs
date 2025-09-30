using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Permissions;

[RemoteService(Name = PermissionsRemoteServiceConsts.RemoteServiceName)]
[Area(PermissionsRemoteServiceConsts.ModuleName)]
[Route("api/permissions/permission")]
public class PermissionController(IPermissionAppService permissionAppService) : AbpControllerBase, IPermissionAppService
{
    protected IPermissionAppService PermissionAppService { get; } = permissionAppService;

    [HttpGet("list")]
    public Task<List<PermissionGrantInfoDto>> GetListAsync(string groupName, string providerKey, string providerName)
    {
        return PermissionAppService.GetListAsync(groupName, providerKey, providerName);
    }

    [HttpGet("groups")]
    public Task<List<PermissionGroupInfoDto>> GetGroupsAsync()
    {
        return PermissionAppService.GetGroupsAsync();
    }

    [HttpPut]
    public Task UpdateAsync(string providerKey, string providerName, UpdatePermissionsDto input)
    {
        return PermissionAppService.UpdateAsync(providerKey, providerName, input);
    }
}