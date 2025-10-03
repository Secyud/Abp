using SecitsDemoApp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SecitsDemoApp;

[DependsOn(
    typeof(SecitsDemoAppEntityFrameworkCoreModule),
    typeof(SecitsDemoAppApplicationContractsModule)
)]
public class SecitsDemoAppDbMigratorSharedModule : AbpModule
{
}