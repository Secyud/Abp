using Secyud.Abp.Settings.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Settings;

[DependsOn(typeof(AbpLocalizationModule),
    typeof(AbpValidationModule),
    typeof(AbpFeaturesModule))]
public class AbpSettingsDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options
            =>
        {
            options.FileSets.AddEmbedded<AbpSettingsDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpSettingsResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/Settings");
        });
    }
}