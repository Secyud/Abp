using Microsoft.Extensions.Options;
using SecitsDemoApp.Pages;
using Secyud.Abp.AspNetCore;
using Secyud.Abp.AspNetCore.Bundling;
using Secyud.Abp.AspNetCore.Components.Bundling;
using Secyud.Abp.AspNetCore.Components.Routing;
using Volo.Abp;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Mvc.Libs;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Modularity;

namespace SecitsDemoApp;

[DependsOn(
    typeof(SecitsDemoAppBlazorModule),
    typeof(SecitsDemoAppHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsServerSecitsThemeModule),
    typeof(AbpAspNetCoreSerilogModule)
)]
public class SecitsDemoAppBlazorServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // PreConfigure<AbpAspNetCoreComponentsWebOptions>(options => { options.IsBlazorWebApp = true; });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options
            =>
        {
            options.StyleBundles.Configure(BlazorSecitsThemeBundles.Styles.Global, bundle
                =>
            {
                bundle.AddExternalFiles("app.css");
            });
        });
        Configure<AbpRouterOptions>(options
            =>
        {
            options.AppAssembly = typeof(SecitsDemoAppBlazorServerModule).Assembly;
        });

        Configure<AbpMvcLibsOptions>(options => { options.CheckLibs = false; });


        context.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var env = context.GetEnvironment();
        var app = context.GetApplicationBuilder();

        app.UseForwardedHeaders();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAbpSecurityHeaders();
        app.UseCors();
        app.UseAntiforgery();
        // app.UseAuthentication();


        app.UseUnitOfWork();
        app.UseDynamicClaims();
        // app.UseAuthorization();
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
        // app.UseConfiguredEndpoints(builder =>
        // {
        //     builder.MapRazorComponents<AppEnter>()
        //         .AddInteractiveServerRenderMode()
        //         .AddAdditionalAssemblies(builder.ServiceProvider.GetRequiredService<IOptions<AbpRouterOptions>>().Value
        //             .AdditionalAssemblies.ToArray());
        // });
    }
}