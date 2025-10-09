using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Secyud.Abp.Permissions.EntityFrameworkCore;

[DependsOn(
    typeof(AbpPermissionsEntityFrameworkCoreModule),
    typeof(AbpPermissionsTestBaseModule))]
public class AbpPermissionsEntityFrameworkCoreTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddEntityFrameworkInMemoryDatabase();

        var databaseName = Guid.NewGuid().ToString();

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(abpDbContextConfigurationContext =>
            {
                abpDbContextConfigurationContext.DbContextOptions.UseInMemoryDatabase(databaseName);
            });
        });

        context.Services.AddAlwaysDisableUnitOfWorkTransaction();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var task = context.ServiceProvider.GetRequiredService<AbpPermissionsDomainModule>().GetInitializeDynamicPermissionsTask();
        if (!task.IsCompleted)
        {
            AsyncHelper.RunSync(() => Awaited(task));
        }
    }

    private static async Task Awaited(Task task)
    {
        await task;
    }
}
