using Volo.Abp.Modularity;

namespace Secyud.Abp.Features;

[DependsOn(
    typeof(AbpFeaturesApplicationModule),
    typeof(AbpFeaturesDomainTestModule)
    )]
public class FeaturesApplicationTestModule : AbpModule
{

}
