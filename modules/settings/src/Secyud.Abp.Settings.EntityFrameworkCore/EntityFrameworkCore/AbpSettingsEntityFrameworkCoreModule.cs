using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Settings.EntityFrameworkCore;

[DependsOn(
    typeof(AbpSettingsDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
    )]
public class AbpSettingsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SettingsDbContext>(options =>
        {
            options.AddDefaultRepositories<ISettingsDbContext>();

            options.AddRepository<Setting, EfCoreSettingRepository>();
            options.AddRepository<SettingDefinitionRecord, EfCoreSettingDefinitionRecordRepository>();
        });
    }
}
