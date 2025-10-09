using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Features;

[DependsOn(
    typeof(AbpFeaturesDomainModule),
    typeof(AbpFeaturesApplicationContractsModule),
    typeof(AbpDddApplicationModule)
    )]
public class AbpFeaturesApplicationModule : AbpModule
{

}
