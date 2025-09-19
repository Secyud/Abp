using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace SecitsDemoApp;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(SecitsDemoAppDomainSharedModule)
)]
public class SecitsDemoAppDomainModule : AbpModule
{

}
