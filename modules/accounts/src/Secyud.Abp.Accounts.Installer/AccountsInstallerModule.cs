using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Accounts;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AccountsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AccountsInstallerModule>();
        });
    }
}
