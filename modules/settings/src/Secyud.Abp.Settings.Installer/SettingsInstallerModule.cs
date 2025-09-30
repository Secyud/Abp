using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Settings;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class SettingsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<SettingsInstallerModule>();
        });
    }
}
