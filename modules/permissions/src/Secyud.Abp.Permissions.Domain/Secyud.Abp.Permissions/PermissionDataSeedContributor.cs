using Secyud.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Permissions;

public class PermissionDataSeedContributor(
    IPermissionDefinitionManager permissionDefinitionManager,
    IPermissionDataSeeder permissionDataSeeder,
    ICurrentTenant currentTenant)
    : IDataSeedContributor, ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; } = currentTenant;
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; } = permissionDefinitionManager;
    protected IPermissionDataSeeder PermissionDataSeeder { get; } = permissionDataSeeder;

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        var multiTenancySide = CurrentTenant.GetMultiTenancySide();
        var permissionNames = (await PermissionDefinitionManager.GetPermissionsAsync())
            .Where(p => p.MultiTenancySide.HasFlag(multiTenancySide))
            .Where(p => !p.Providers.Any() || p.Providers.Contains(PermissionProviderNames.Role))
            .Select(p => p.Name)
            .ToArray();

        await PermissionDataSeeder.SeedAsync(
            PermissionProviderNames.Role,
            "admin",
            permissionNames,
            context.TenantId
        );
    }
}