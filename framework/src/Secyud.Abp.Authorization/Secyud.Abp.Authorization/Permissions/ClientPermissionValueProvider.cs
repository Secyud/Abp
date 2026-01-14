using Volo.Abp;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Secyud.Abp.Authorization.Permissions;

public class ClientPermissionValueProvider(
    IPermissionStore permissionStore,
    ICurrentTenant currentTenant)
    : PermissionValueProviderBase(permissionStore)
{
    public const string ProviderName = "C";
    public override string Name => ProviderName;
    protected ICurrentTenant CurrentTenant { get; } = currentTenant;

    public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
    {
        var clientId = context.Principal?.FindFirst(AbpClaimTypes.ClientId)?.Value;

        if (clientId == null)
        {
            return PermissionGrantResult.Undefined;
        }

        using var change = CurrentTenant.Change(null);

        return await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, clientId);
    }

    public override async Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
    {
        var permissionNames = context.Permissions.Select(x => x.Name).Distinct().ToArray();
        Check.NotNullOrEmpty(permissionNames, nameof(permissionNames));

        var clientId = context.Principal?.FindFirst(AbpClaimTypes.ClientId)?.Value;
        if (clientId == null)
        {
            return new MultiplePermissionGrantResult(permissionNames);
        }

        using (CurrentTenant.Change(null))
        {
            return await PermissionStore.IsGrantedAsync(permissionNames, Name, clientId);
        }
    }
}