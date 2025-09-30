using Secyud.Abp.Settings;
using Secyud.Abp.Permissions;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SecitsDemoApp;

[DependsOn(
    typeof(AbpSettingsApplicationContractsModule),
    typeof(AbpPermissionsApplicationContractsModule),
    typeof(SecitsDemoAppDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class SecitsDemoAppApplicationContractsModule : AbpModule
{

}
