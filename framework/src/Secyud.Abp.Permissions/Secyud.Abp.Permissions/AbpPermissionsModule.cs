using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Permissions;

[DependsOn(typeof(AbpAuthorizationModule))]
public class AbpPermissionsModule : AbpModule
{
}