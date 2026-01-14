using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Authorization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Features;

[DependsOn(
    typeof(AbpLocalizationModule),
    typeof(AbpMultiTenancyModule),
    typeof(AbpValidationModule),
    typeof(AbpAuthorizationAbstractionsModule)
)]
public class AbpFeaturesModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(FeatureInterceptorRegistrar.RegisterIfNeeded);
        AutoAddDefinitionProviders(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<AbpFeatureOptions>(options =>
        {
            options.ValueProviders.Add<DefaultValueFeatureValueProvider>();
            options.ValueProviders.Add<EditionFeatureValueProvider>();
            options.ValueProviders.Add<TenantFeatureValueProvider>();
        });

        Configure<AbpVirtualFileSystemOptions>(options
            =>
        {
            options.FileSets.AddEmbedded<AbpFeatureResource>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpFeatureResource>("en")
                .AddVirtualJson("/Secyud/Abp/Features/Localization");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Secyud.Feature", typeof(AbpFeatureResource));
        });
    }

    private static void AutoAddDefinitionProviders(IServiceCollection services)
    {
        var definitionProviders = new List<Type>();

        services.OnRegistered(context =>
        {
            if (typeof(IFeatureDefinitionProvider).IsAssignableFrom(context.ImplementationType))
            {
                definitionProviders.Add(context.ImplementationType);
            }
        });

        services.Configure<AbpFeatureOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(definitionProviders);
        });
    }
}