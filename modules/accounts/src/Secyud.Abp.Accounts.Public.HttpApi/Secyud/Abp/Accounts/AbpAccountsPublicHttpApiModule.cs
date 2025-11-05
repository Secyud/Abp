using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Account.Localization;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Accounts;

[DependsOn(
    typeof(AbpAccountsPublicApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpAccountsPublicHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder => { mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountsPublicHttpApiModule).Assembly); });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpAccountsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });

        Configure<AbpAspNetCoreMvcOptions>(options => { options.ConventionalControllers.FormBodyBindingIgnoredTypes.Add(typeof(ProfilePictureInput)); });
    }
}