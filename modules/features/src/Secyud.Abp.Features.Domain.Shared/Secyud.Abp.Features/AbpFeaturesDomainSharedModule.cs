using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Features.JsonConverters;
using Secyud.Abp.Features.Localization;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Features;

[DependsOn(
    typeof(AbpValidationModule),
    typeof(AbpJsonSystemTextJsonModule)
)]
public class AbpFeaturesDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options
            =>
        {
            options.FileSets.AddEmbedded<AbpFeaturesDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpFeaturesResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("Secyud/Abp/Features/Localization/DomainShared");
        });

        Configure<AbpExceptionLocalizationOptions>(options
            =>
        {
            options.MapCodeNamespace("Secyud.Abp.Features", typeof(AbpFeaturesResource));
        });

        var valueValidatorFactoryOptions = context.Services
            .GetPreConfigureActions<ValueValidatorFactoryOptions>();
        Configure<ValueValidatorFactoryOptions>(options
            =>
        {
            valueValidatorFactoryOptions.Configure(options);
        });

        Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.JsonSerializerOptions.Converters
                .Add(new StringValueTypeJsonConverter(valueValidatorFactoryOptions.Configure()));
        });
    }
}