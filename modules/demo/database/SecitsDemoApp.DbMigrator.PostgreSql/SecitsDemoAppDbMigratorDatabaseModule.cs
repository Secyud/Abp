using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SecitsDemoApp;

[DependsOn(
    typeof(SecitsDemoAppDbMigratorSharedModule)
)]
public class SecitsDemoAppDbMigratorDatabaseModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbContextOptions>(options
            =>
        {
            options.UseNpgsql();
        });
    }
}