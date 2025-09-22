using Secyud.Abp.AspNetCore;
using Secyud.Abp.AspNetCore.Components.Routing;
using Volo.Abp;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Modularity;

namespace SecitsDemoApp;

[DependsOn(
    typeof(SecitsDemoAppBlazorModule),
    typeof(AbpAspNetCoreComponentsServerSecitsThemeModule),
    typeof(AbpAspNetCoreSerilogModule)
)]
public class SecitsDemoAppBlazorServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpRouterOptions>(options =>
        {
            options.AppAssembly = typeof(SecitsDemoAppBlazorServerModule).Assembly;
        });
        
        context.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var env = context.GetEnvironment();
        var app = context.GetApplicationBuilder();

        // if (env.IsDevelopment())
        // {
        //     app.UseDeveloperExceptionPage();
        // }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        // app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}