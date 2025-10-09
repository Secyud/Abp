using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Tenants;

[DependsOn(
    typeof(AbpTenantsDomainModule),
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule)
    )]
public class AbpTenantsTestBaseModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        using var scope = context.ServiceProvider.CreateScope();
        scope.ServiceProvider
            .GetRequiredService<AbpTenantsTestDataBuilder>()
            .Build();
    }
}
