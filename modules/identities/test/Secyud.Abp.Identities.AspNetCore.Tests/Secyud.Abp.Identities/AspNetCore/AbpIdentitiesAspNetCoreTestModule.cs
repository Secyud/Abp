using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Identities.AspNetCore;

[DependsOn(
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpIdentitiesDomainTestModule),
    typeof(AbpAspNetCoreTestBaseModule),
    typeof(AbpAspNetCoreMvcModule)
)]
public class AbpIdentitiesAspNetCoreTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<IMvcBuilder>(builder =>
        {
            builder.PartManager.ApplicationParts.Add(new AssemblyPart(typeof(AbpIdentitiesAspNetCoreTestModule).Assembly));
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpIdentityOptions>(options =>
        {
            options.ExternalLoginProviders.Add<FakeExternalLoginProvider>(FakeExternalLoginProvider.Name);
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseConfiguredEndpoints();
    }
}
