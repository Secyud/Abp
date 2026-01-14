using System.Security.Claims;
using System.Security.Principal;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.SimpleStateChecking;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionChecker(
    ICurrentPrincipalAccessor principalAccessor,
    ICurrentTenant currentTenant,
    PermissionDefinitionManager permissionDefinitionManager,
    IPermissionValueProviderManager permissionValueProviderManager,
    ISimpleStateCheckerManager<PermissionDefinition> stateCheckerManager)
    : IPermissionChecker, ITransientDependency
{
    protected PermissionDefinitionManager PermissionDefinitionManager { get; } = permissionDefinitionManager;
    protected ICurrentPrincipalAccessor PrincipalAccessor { get; } = principalAccessor;
    protected ICurrentTenant CurrentTenant { get; } = currentTenant;
    protected IPermissionValueProviderManager PermissionValueProviderManager { get; } = permissionValueProviderManager;
    protected ISimpleStateCheckerManager<PermissionDefinition> StateCheckerManager { get; } = stateCheckerManager;

    public virtual async Task<PermissionGrantResult> IsGrantedAsync(string name)
    {
        return await IsGrantedAsync(PrincipalAccessor.Principal, name);
    }

    public virtual async Task<PermissionGrantResult> IsGrantedAsync(
        ClaimsPrincipal? claimsPrincipal,
        string name)
    {
        Check.NotNull(name, nameof(name));

        var permission = await PermissionDefinitionManager.GetOrNullAsync(name);
        if (permission is null)
        {
            return PermissionGrantResult.Undefined;
        }

        if (!await StateCheckerManager.IsEnabledAsync(permission))
        {
            return PermissionGrantResult.Prohibited;
        }

        var multiTenancySide = claimsPrincipal?.GetMultiTenancySide()
                               ?? CurrentTenant.GetMultiTenancySide();

        if (!permission.MultiTenancySide.HasFlag(multiTenancySide))
        {
            return PermissionGrantResult.Prohibited;
        }

        var res = PermissionGrantResult.Unset;
        var context = new PermissionValueCheckContext(permission, claimsPrincipal);
        foreach (var provider in PermissionValueProviderManager.ValueProviders)
        {
            if (context.Permission.Providers.Count != 0 &&
                !context.Permission.Providers.Contains(provider.Name))
            {
                continue;
            }

            var result = await provider.CheckAsync(context);

            switch (result)
            {
                case PermissionGrantResult.Granted:
                    res = PermissionGrantResult.Granted;
                    break;
                case PermissionGrantResult.Prohibited:
                    return PermissionGrantResult.Prohibited;
            }
        }

        return res;
    }

    public virtual async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names)
    {
        return await IsGrantedAsync(PrincipalAccessor.Principal, names);
    }

    public virtual async Task<MultiplePermissionGrantResult> IsGrantedAsync(ClaimsPrincipal? claimsPrincipal,
        string[] names)
    {
        Check.NotNull(names, nameof(names));

        var result = new MultiplePermissionGrantResult();
        if (names.Length == 0) return result;

        var multiTenancySide = claimsPrincipal?.GetMultiTenancySide() ??
                               CurrentTenant.GetMultiTenancySide();

        var permissionDefinitions = new List<PermissionDefinition>();
        foreach (var name in names)
        {
            var permission = await PermissionDefinitionManager.GetOrNullAsync(name);
            if (permission == null)
            {
                result.Result.Add(name, PermissionGrantResult.Undefined);
                continue;
            }

            result.Result.Add(name, PermissionGrantResult.Unset);

            if (await StateCheckerManager.IsEnabledAsync(permission) &&
                permission.MultiTenancySide.HasFlag(multiTenancySide))
            {
                permissionDefinitions.Add(permission);
            }
        }

        foreach (var provider in PermissionValueProviderManager.ValueProviders)
        {
            var permissions = permissionDefinitions
                .Where(x => !x.Providers.Any() || x.Providers.Contains(provider.Name))
                .ToList();

            if (permissions.IsNullOrEmpty())
            {
                continue;
            }

            var context = new PermissionValuesCheckContext(permissions, claimsPrincipal);

            var multipleResult = await provider.CheckAsync(context);

            foreach (var (key, value) in multipleResult.Result)
            {
                switch (value)
                {
                    case PermissionGrantResult.Granted:
                    {
                        result.Result[key] = PermissionGrantResult.Granted;
                        break;
                    }
                    case PermissionGrantResult.Prohibited:
                        result.Result[key] = PermissionGrantResult.Prohibited;
                        permissionDefinitions.RemoveAll(x => x.Name == key);
                        break;
                    case PermissionGrantResult.Undefined:
                    case PermissionGrantResult.Unset:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        return result;
    }
}