using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

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