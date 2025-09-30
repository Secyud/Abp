using Secyud.Abp.Permissions;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace SecitsDemoApp;

[DependsOn(
    typeof(AbpPermissionsDomainModule),
    typeof(AbpDddDomainModule),
    typeof(SecitsDemoAppDomainSharedModule)
)]
public class SecitsDemoAppDomainModule : AbpModule
{

}
