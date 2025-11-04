using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Permissions;

[DependsOn(typeof(AbpAuthorizationAbstractionsModule))]
public class AbpPermissionsModule : AbpModule
{
}