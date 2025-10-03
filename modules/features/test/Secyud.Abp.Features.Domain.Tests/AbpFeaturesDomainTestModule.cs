using Secyud.Abp.Features.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Features;

[DependsOn(
    typeof(AbpFeaturesEntityFrameworkCoreTestModule)
    )]
public class AbpFeaturesDomainTestModule : AbpModule
{

}
