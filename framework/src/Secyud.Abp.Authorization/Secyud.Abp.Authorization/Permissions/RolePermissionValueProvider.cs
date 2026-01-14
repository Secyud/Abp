using Volo.Abp;
using Volo.Abp.Security.Claims;

namespace Secyud.Abp.Authorization.Permissions;

public class RolePermissionValueProvider(IPermissionStore permissionStore)
    : PermissionValueProviderBase(permissionStore)
{
    public const string ProviderName = "R";
    public override string Name => ProviderName;

    public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
    {
        var roles = context.Principal?.FindAll(AbpClaimTypes.Role).Select(c => c.Value).ToArray();

        if (roles == null || roles.Length == 0)
        {
            return PermissionGrantResult.Undefined;
        }

        foreach (var role in roles.Distinct())
        {
            var result = await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, role);
            if (result == PermissionGrantResult.Granted)
            {
                return PermissionGrantResult.Granted;
            }
        }

        return PermissionGrantResult.Unset;
    }

    public override async Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
    {
        var permissionNames = context.Permissions.Select(x => x.Name).Distinct().ToList();
        Check.NotNullOrEmpty(permissionNames, nameof(permissionNames));

        var result = new MultiplePermissionGrantResult(permissionNames.ToArray());

        var roles = context.Principal?.FindAll(AbpClaimTypes.Role).Select(c => c.Value).ToArray();
        if (roles == null || roles.Length == 0)
        {
            return result;
        }

        foreach (var role in roles.Distinct())
        {
            var multipleResult = await PermissionStore.IsGrantedAsync(
                permissionNames.ToArray(), Name, role);

            foreach (var (key, value) in multipleResult.Result)
            {
                if (value is PermissionGrantResult.Unset)
                {
                    continue;
                }

                result.Result[key] = value;
                permissionNames.RemoveAll(x => x == key);
            }

            if (permissionNames.IsNullOrEmpty())
            {
                break;
            }
        }

        return result;
    }
}