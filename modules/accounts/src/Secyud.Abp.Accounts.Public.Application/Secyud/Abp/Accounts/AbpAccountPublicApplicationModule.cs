using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.Imaging;
using Volo.Abp.Modularity;
using Volo.Abp.Sms;
using Volo.Abp.Timing;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Accounts;

[DependsOn(
    typeof(AbpAccountsSharedApplicationModule),
    typeof(AbpSmsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpBlobStoringModule),
    typeof(AbpAccountsPublicApplicationContractsModule),
    typeof(AbpImagingImageSharpModule),
    typeof(AbpTimingModule)
    )]
public class AbpAccountsPublicApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].Urls[AccountUrlNames.PasswordReset] = "Account/ResetPassword";
            options.Applications["MVC"].Urls[AccountUrlNames.EmailConfirmation] = "Account/EmailConfirmation";
        });

        context.Services.AddAutoMapperObjectMapper<AbpAccountsPublicApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpAccountPubicApplicationModuleAutoMapperProfile>();
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountsPublicApplicationModule>();
        });

        context.Services.AddHttpClient();
    }
}
