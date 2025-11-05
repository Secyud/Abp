using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Accounts.Pages.Account;
using Secyud.Abp.AspNetCore.Components;
using Secyud.Abp.AspNetCore.Components.Routing;
using Secyud.Abp.ObjectExtending;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;

namespace Secyud.Abp.Accounts;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAccountsPublicApplicationContractsModule),
    typeof(AbpAccountsPublicBlazorSharedModule)
    )]
public class AbpAccountsPublicBlazorModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpAccountsPublicBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpAccountBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AbpAccountBlazorUserMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AbpAccountsPublicBlazorModule).Assembly);
        });
    }
    
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.User,
                    editFormTypes: [typeof(PersonalInfoModel)]
                );
        });
    }
}
