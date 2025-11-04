namespace Secyud.Abp.Identities.AspNetCore;

public class AbpIdentityAspNetCoreTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<AbpIdentitiesAspNetCoreTestModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
