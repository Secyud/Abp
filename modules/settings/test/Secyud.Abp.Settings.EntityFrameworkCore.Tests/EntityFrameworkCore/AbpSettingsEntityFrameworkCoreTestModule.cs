using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Secyud.Abp.Settings.EntityFrameworkCore;

[DependsOn(
    typeof(AbpSettingsTestBaseModule),
    typeof(AbpSettingsEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
    )]
public class AbpSettingsEntityFrameworkCoreTestModule : AbpModule
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

        new SettingsDbContext(
            new DbContextOptionsBuilder<SettingsDbContext>().UseSqlite(connection).Options
        ).GetService<IRelationalDatabaseCreator>().CreateTables();

        return connection;
    }
}
