using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Permissions;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class PermissionsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PermissionsInstallerModule>();
        });
    }
}
