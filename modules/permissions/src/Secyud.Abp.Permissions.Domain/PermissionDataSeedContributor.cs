using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Permissions;

public class PermissionDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; }
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
    protected IPermissionDataSeeder PermissionDataSeeder { get; }

    public PermissionDataSeedContributor(
        IPermissionDefinitionManager permissionDefinitionManager,
        IPermissionDataSeeder permissionDataSeeder,
        ICurrentTenant currentTenant)
    {
        PermissionDefinitionManager = permissionDefinitionManager;
        PermissionDataSeeder = permissionDataSeeder;
        CurrentTenant = currentTenant;
    }

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        var multiTenancySide = CurrentTenant.GetMultiTenancySide();
        var permissionNames = (await PermissionDefinitionManager.GetPermissionsAsync())
            .Where(p => p.MultiTenancySide.HasFlag(multiTenancySide))
            .Where(p => !p.Providers.Any() || p.Providers.Contains(RolePermissionValueProvider.ProviderName))
            .Select(p => p.Name)
            .ToArray();

        await PermissionDataSeeder.SeedAsync(
            RolePermissionValueProvider.ProviderName,
            "admin",
            permissionNames,
            context?.TenantId
        );
    }
}
