using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Secyud.Abp.Features.EntityFrameworkCore;

[DependsOn(
    typeof(FeaturesTestBaseModule),
    typeof(AbpFeaturesEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
    )]
public class AbpFeaturesEntityFrameworkCoreTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var sqliteConnection = CreateDatabaseAndGetConnection();

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(abpDbContextConfigurationContext =>
            {
                abpDbContextConfigurationContext.DbContextOptions.UseSqlite(sqliteConnection);
            });
        });

        context.Services.AddAlwaysDisableUnitOfWorkTransaction();
    }

    private static SqliteConnection CreateDatabaseAndGetConnection()
    {
        var connection = new AbpUnitTestSqliteConnection("Data Source=:memory:");
        connection.Open();

        new FeaturesDbContext(
            new DbContextOptionsBuilder<FeaturesDbContext>().UseSqlite(connection).Options
        ).GetService<IRelationalDatabaseCreator>().CreateTables();

        return connection;
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var task = context.ServiceProvider.GetRequiredService<AbpFeaturesDomainModule>().GetInitializeDynamicFeaturesTask();
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
