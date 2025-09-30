using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Localization;

namespace Secyud.Abp.Permissions;

[Authorize]
public class PermissionAppService(
    IPermissionManager manager,
    ILocalizableStringSerializer serializer)
    : ApplicationService, IPermissionAppService
{
    protected IPermissionManager Manager { get; } = manager;
    public ILocalizableStringSerializer Serializer { get; } = serializer;

    public async Task<List<PermissionGrantInfoDto>> GetListAsync(string groupName,
        string providerName, string providerKey)
    {
        var infos = await Manager.GetListAsync(groupName, providerName, providerKey);
        infos = infos.Where(u => u.IsEnabled).ToList();
        foreach (var info in infos)
        {
            if (info.DisplayName is null) continue;
            var localizableString = Serializer.Deserialize(info.DisplayName);
            info.DisplayName = localizableString.Localize(StringLocalizerFactory);
        }

        var result = new List<PermissionGrantInfoDto>();
        return ObjectMapper.Map(infos, result);
    }

    public async Task<List<PermissionGroupInfoDto>> GetGroupsAsync()
    {
        var infos = await Manager.GetGroupsAsync();
        foreach (var info in infos)
        {
            if (info.DisplayName is null) continue;
            var localizableString = Serializer.Deserialize(info.DisplayName);
            info.DisplayName = localizableString.Localize(StringLocalizerFactory);
        }

        var result = new List<PermissionGroupInfoDto>();
        return ObjectMapper.Map(infos, result);
    }

    public virtual async Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
    {
        var group = input.Permissions.GroupBy(u => u.IsGranted).ToList();
        var grantedPermissions = group.FirstOrDefault(u => u.Key)?
            .Select(u => u.Name).ToArray() ?? [];
        var deniedPermissions = group.FirstOrDefault(u => !u.Key)?
            .Select(u => u.Name).ToArray() ?? [];

        await Manager.UpdateAsync(providerName, providerKey, grantedPermissions, deniedPermissions);
    }
}