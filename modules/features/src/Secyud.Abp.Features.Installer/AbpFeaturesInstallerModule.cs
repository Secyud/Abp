using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
)]
public class AbpFeaturesInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options
            =>
        {
            options.FileSets.AddEmbedded<AbpFeaturesInstallerModule>();
        });
    }
}