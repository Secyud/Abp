using Secyud.Abp.Permissions.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SecitsDemoApp.EntityFrameworkCore;

[DependsOn(
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
