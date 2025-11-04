using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Identities.EntityFrameworkCore;
using Secyud.Abp.Permissions.Identity;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Secyud.Abp.Identities;

[DependsOn(
    typeof(AbpIdentitiesEntityFrameworkCoreTestModule),
    typeof(AbpIdentitiesTestBaseModule),
    typeof(AbpPermissionsDomainIdentityModule)
    )]
public class AbpIdentitiesDomainTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.AutoEventSelectors.Add<IdentityUser>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        using var scope = context.ServiceProvider.CreateScope();
        AsyncHelper.RunSync(() => scope.ServiceProvider
            .GetRequiredService<TestPermissionDataBuilder>()
            .Build());
    }
}
