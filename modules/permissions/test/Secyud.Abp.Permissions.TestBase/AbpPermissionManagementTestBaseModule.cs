using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Secyud.Abp.Permissions;

[DependsOn(
    typeof(AbpPermissionsDomainModule),
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule)
)]
public class AbpPermissionsTestBaseModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<AbpPermissionOptions>(options => { options.ValueProviders.Add<TestPermissionValueProvider>(); });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() =>
        {
            using var scope = context.ServiceProvider.CreateScope();
            return scope.ServiceProvider
                .GetRequiredService<PermissionTestDataBuilder>()
                .BuildAsync();
        });
    }
}