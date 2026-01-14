using Volo.Abp;
using Volo.Abp.Security.Claims;

namespace Secyud.Abp.Authorization.Permissions;

public class UserPermissionValueProvider(IPermissionStore permissionStore)
    : PermissionValueProviderBase(permissionStore)
{
    public const string ProviderName = "U";

    public override string Name => ProviderName;

    public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
    {
        var userId = context.Principal?.FindFirst(AbpClaimTypes.UserId)?.Value;

        if (userId is null)
        {
            return PermissionGrantResult.Undefined;
        }

        return await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, userId);
    }

    public override async Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
    {
        var permissionNames = context.Permissions.Select(x => x.Name).Distinct().ToArray();
        Check.NotNullOrEmpty(permissionNames, nameof(permissionNames));

        var userId = context.Principal?.FindFirst(AbpClaimTypes.UserId)?.Value;
        if (userId == null)
        {
            return new MultiplePermissionGrantResult(permissionNames);
        }

        return await PermissionStore.IsGrantedAsync(permissionNames, Name, userId);
    }
}