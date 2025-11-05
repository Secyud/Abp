using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Account;
using Secyud.Abp.Account.Localization;
using Secyud.Abp.Accounts.Settings;
using Secyud.Abp.AspNetCore.Components.Routing;
using Secyud.Abp.Settings;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Accounts;

[DependsOn(
    typeof(AbpAccountsAdminApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpSettingsBlazorModule)
)]
public class AbpAccountAdminBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpAccountAdminBlazorModule>();

        Configure<AbpAutoMapperOptions>(options => { options.AddProfile<AbpAccountAdminBlazorAutoMapperProfile>(validate: true); });

        Configure<AbpRouterOptions>(options => { options.AdditionalAssemblies.Add(typeof(AbpAccountAdminBlazorModule).Assembly); });

        Configure<SettingsComponentOptions>(options => { options.Contributors.Add(new AbpAccountAdminSettingManagementComponentContributor()); });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpAccountsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}