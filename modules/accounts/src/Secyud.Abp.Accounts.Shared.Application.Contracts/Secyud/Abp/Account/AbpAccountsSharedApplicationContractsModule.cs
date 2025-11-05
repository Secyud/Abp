using Secyud.Abp.Account.Localization;
using Secyud.Abp.Identities;
using Secyud.Abp.Identities.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Account;

[DependsOn(
    typeof(AbpIdentityApplicationContractsModule)
)]
public class AbpAccountsSharedApplicationContractsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<AbpAccountsSharedApplicationContractsModule>(); });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpAccountsResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddBaseTypes(typeof(AbpIdentitiesResource))
                .AddVirtualJson("/Secyud/Abp/Accounts/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options
            =>
        {
            options.MapCodeNamespace("Secyud.Accounts", typeof(AbpAccountsResource));
        });
    }
}