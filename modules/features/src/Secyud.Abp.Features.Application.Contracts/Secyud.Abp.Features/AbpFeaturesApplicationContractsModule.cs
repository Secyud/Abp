using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Features;

[DependsOn(
    typeof(AbpFeaturesDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule),
    typeof(AbpJsonModule)
    )]
public class AbpFeaturesApplicationContractsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpFeaturesApplicationContractsModule>();
        });
    }
}