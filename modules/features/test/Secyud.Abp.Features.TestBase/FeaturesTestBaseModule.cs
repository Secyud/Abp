using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Features;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Secyud.Abp.Features;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpFeaturesDomainModule)
    )]
public class FeaturesTestBaseModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysAllowAuthorization();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FeaturesOptions>(options =>
        {
            options.Providers.InsertBefore(typeof(TenantFeaturesProvider), typeof(NextTenantFeaturesProvider));

            //TODO: Any value can pass. After completing the permission unit test, look at it again.
            options.ProviderPolicies[EditionFeatureValueProvider.ProviderName] = EditionFeatureValueProvider.ProviderName;
        });
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        using (var scope = context.ServiceProvider.CreateScope())
        {
            AsyncHelper.RunSync(() => scope.ServiceProvider
                .GetRequiredService<FeaturesTestDataBuilder>()
                .BuildAsync());
        }
    }
}
