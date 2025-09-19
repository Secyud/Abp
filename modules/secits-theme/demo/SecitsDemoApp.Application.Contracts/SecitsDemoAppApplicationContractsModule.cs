using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SecitsDemoApp;

[DependsOn(
    typeof(SecitsDemoAppDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class SecitsDemoAppApplicationContractsModule : AbpModule
{

}
