using Secyud.Abp.Account;
using Secyud.Abp.ObjectExtending;
using Volo.Abp;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Accounts;

[DependsOn(
    typeof(AbpAccountsSharedApplicationContractsModule),
    typeof(AbpEmailingModule)
    )]
public class AbpAccountsPublicApplicationContractsModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountsPublicApplicationContractsModule>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
    
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.User,
                getApiTypes: new[] { typeof(ProfileDto) },
                updateApiTypes: new[] { typeof(UpdateProfileDto) }
            );
        });
    }
}
