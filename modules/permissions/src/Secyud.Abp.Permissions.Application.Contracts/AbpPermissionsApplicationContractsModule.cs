using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Permissions;

[DependsOn(typeof(AbpDddApplicationContractsModule))]
[DependsOn(typeof(AbpPermissionsDomainSharedModule))]
[DependsOn(typeof(AbpAuthorizationAbstractionsModule))]
public class AbpPermissionsApplicationContractsModule : AbpModule
{

}
