using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
)]
public class AbpPermissionsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPermissionsInstallerModule>();
        });
    }
}