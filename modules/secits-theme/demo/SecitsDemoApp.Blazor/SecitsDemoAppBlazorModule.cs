using SecitsDemoApp.Menus;
using Secyud.Abp.AspNetCore;
using Secyud.Abp.AspNetCore.Components.Routing;
using Volo.Abp;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace SecitsDemoApp;

[DependsOn(
    typeof(SecitsDemoAppApplicationContractsModule),
    typeof(AbpAspNetCoreComponentsServerSecitsThemeModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAutoMapperModule)
)]
public class SecitsDemoAppBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<SecitsDemoAppBlazorModule>();

        Configure<AbpAutoMapperOptions>(options => { options.AddProfile<SecitsDemoAppBlazorAutoMapperProfile>(validate: true); });

        Configure<AbpNavigationOptions>(options => { options.MenuContributors.Add(new SecitsDemoAppMenuContributor()); });

        Configure<AbpRouterOptions>(options =>
        {
            options.AppAssembly = typeof(SecitsDemoAppBlazorModule).Assembly;
        });
        
        context.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var env = context.GetEnvironment();
        var app = context.GetApplicationBuilder();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

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