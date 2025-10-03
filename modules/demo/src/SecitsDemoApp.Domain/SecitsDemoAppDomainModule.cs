using Secyud.Abp.Tenants;
using Secyud.Abp.Features;
using Secyud.Abp.Settings;
using Secyud.Abp.Permissions;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace SecitsDemoApp;

[DependsOn(
    typeof(AbpTenantsDomainModule),
    typeof(AbpFeaturesDomainModule),
    typeof(AbpSettingsDomainModule),
    typeof(AbpPermissionsDomainModule),
    typeof(AbpDddDomainModule),
    typeof(SecitsDemoAppDomainSharedModule)
)]
public class SecitsDemoAppDomainModule : AbpModule
{

}
