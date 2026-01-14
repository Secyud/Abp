using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Authorization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.GlobalFeatures;

[DependsOn(
    typeof(AbpLocalizationModule),
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpAuthorizationAbstractionsModule)
)]
public class AbpGlobalFeaturesModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(GlobalFeatureInterceptorRegistrar.RegisterIfNeeded);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpGlobalFeatureResource>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpGlobalFeatureResource>("en")
                .AddVirtualJson("/Secyud/Abp/GlobalFeatures/Localization");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Secyud.GlobalFeature", typeof(AbpGlobalFeatureResource));
        });
    }
}
