using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Features.JsonConverters;
using Secyud.Abp.Features.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Features;

[DependsOn(
    typeof(AbpFeaturesApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpFeaturesHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpFeaturesHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpFeaturesResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });

        var valueValidatorFactoryOptions = context.Services.ExecutePreConfiguredActions<ValueValidatorFactoryOptions>();
        Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.AddIfNotContains(new StringValueTypeJsonConverter(valueValidatorFactoryOptions));
        });
    }
}
