using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Secyud.Abp.Settings;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(AbpSettingsDomainModule))]
public class AbpSettingsTestBaseModule : AbpModule
{
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
                .GetRequiredService<SettingTestDataBuilder>()
                .BuildAsync();
        });
    }
}