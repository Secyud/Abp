using Microsoft.Extensions.DependencyInjection;
using Secyud.Secits.Blazor;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Authorization;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Secits.Blazor;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpGlobalFeaturesModule),
    typeof(AbpFeaturesModule)
)]
public class AbpSecitsBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureSecitsBlazor(context);
    }

    private void ConfigureSecitsBlazor(ServiceConfigurationContext context)
    {
        context.Services.AddSecitsBlazor();

        context.Services.AddSingleton(typeof(AbpBlazorMessageLocalizerHelper<>));
    }
}