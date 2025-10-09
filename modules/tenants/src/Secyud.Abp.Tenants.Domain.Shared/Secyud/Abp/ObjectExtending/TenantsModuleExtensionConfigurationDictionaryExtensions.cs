using Volo.Abp.ObjectExtending.Modularity;

namespace Secyud.Abp.ObjectExtending;

public static class TenantsModuleExtensionConfigurationDictionaryExtensions
{
    public static ModuleExtensionConfigurationDictionary ConfigureTenants(
        this ModuleExtensionConfigurationDictionary modules,
        Action<TenantsModuleExtensionConfiguration> configureAction)
    {
        return modules.ConfigureModule(
            TenantsModuleExtensionConsts.ModuleName,
            configureAction
        );
    }
}
