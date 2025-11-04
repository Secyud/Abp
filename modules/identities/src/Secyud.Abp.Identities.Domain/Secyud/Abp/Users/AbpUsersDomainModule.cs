using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Users;

namespace Secyud.Abp.Users;

[DependsOn(
    typeof(AbpUsersDomainSharedModule),
    typeof(AbpUsersAbstractionModule),
    typeof(AbpDddDomainModule)
    )]
public class AbpUsersDomainModule : AbpModule
{

}
