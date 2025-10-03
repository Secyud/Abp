using Volo.Abp.DependencyInjection;

namespace SecitsDemoApp;

/* This is used if database provider does't define
 * ISecitsDemoAppDbSchemaMigrator implementation.
 */
public class NullSecitsDemoAppDbSchemaMigrator : ISecitsDemoAppDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
