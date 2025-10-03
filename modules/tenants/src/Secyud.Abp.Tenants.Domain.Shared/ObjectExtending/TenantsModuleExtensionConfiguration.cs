using Volo.Abp.ObjectExtending.Modularity;

// ReSharper disable CheckNamespace
namespace Secyud.Abp.ObjectExtending;

public class TenantsModuleExtensionConfiguration : ModuleExtensionConfiguration
{
    public TenantsModuleExtensionConfiguration ConfigureTenant(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            TenantsModuleExtensionConsts.EntityNames.Tenant,
            configureAction
        );
    }
}