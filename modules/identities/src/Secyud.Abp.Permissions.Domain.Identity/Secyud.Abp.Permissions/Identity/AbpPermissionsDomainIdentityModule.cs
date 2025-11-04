using Secyud.Abp.Identities;
using Volo.Abp.Modularity;
using Volo.Abp.Users;

namespace Secyud.Abp.Permissions.Identity;

[DependsOn(
    typeof(AbpIdentitiesDomainSharedModule),
    typeof(AbpPermissionsDomainModule),
    typeof(AbpUsersAbstractionModule)
)]
public class AbpPermissionsDomainIdentityModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}
