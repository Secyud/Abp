using Secyud.Abp.Tenants;
using Secyud.Abp.Features;
using Secyud.Abp.Settings;
using Secyud.Abp.Permissions;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SecitsDemoApp;

[DependsOn(
    typeof(AbpTenantsApplicationContractsModule),
    typeof(AbpFeaturesApplicationContractsModule),
    typeof(AbpSettingsApplicationContractsModule),
    typeof(AbpPermissionsApplicationContractsModule),
    typeof(SecitsDemoAppDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class SecitsDemoAppApplicationContractsModule : AbpModule
{

}
