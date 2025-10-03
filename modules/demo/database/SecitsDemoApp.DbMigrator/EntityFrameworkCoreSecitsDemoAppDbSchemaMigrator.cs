using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SecitsDemoApp.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace SecitsDemoApp;

public class EntityFrameworkCoreSecitsDemoAppDbSchemaMigrator(IServiceProvider serviceProvider)
    : ISecitsDemoAppDbSchemaMigrator, ITransientDependency
{
    public async Task MigrateAsync()
    {
        await serviceProvider.GetRequiredService<SecitsDemoAppMigratorDbContext>()
            .Database
            .MigrateAsync();
    }
}