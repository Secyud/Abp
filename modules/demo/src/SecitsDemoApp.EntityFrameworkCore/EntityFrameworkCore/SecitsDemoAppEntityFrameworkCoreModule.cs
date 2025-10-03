using Secyud.Abp.Tenants.EntityFrameworkCore;
using Secyud.Abp.Features.EntityFrameworkCore;
using Secyud.Abp.Settings.EntityFrameworkCore;
using Secyud.Abp.Permissions.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SecitsDemoApp.EntityFrameworkCore;

[DependsOn(
    typeof(AbpTenantsEntityFrameworkCoreModule),
    typeof(AbpFeaturesEntityFrameworkCoreModule),
    typeof(AbpSettingsEntityFrameworkCoreModule),
    typeof(AbpPermissionsEntityFrameworkCoreModule),
    typeof(SecitsDemoAppDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class SecitsDemoAppEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SecitsDemoAppDbContext>(options =>
        {
            options.AddDefaultRepositories(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
