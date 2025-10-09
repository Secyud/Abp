using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Features.EntityFrameworkCore;

[DependsOn(
    typeof(AbpFeaturesDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class AbpFeaturesEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<FeaturesDbContext>(options =>
        {
            options.AddRepository<FeatureGroupDefinitionRecord, EfCoreFeatureGroupDefinitionRecordRepository>();
            options.AddRepository<FeatureDefinitionRecord, EfCoreFeatureDefinitionRecordRepository>();
            options.AddDefaultRepositories<IFeaturesDbContext>();

            options.AddRepository<FeatureValue, EfCoreFeatureValueRepository>();
        });
    }
}
