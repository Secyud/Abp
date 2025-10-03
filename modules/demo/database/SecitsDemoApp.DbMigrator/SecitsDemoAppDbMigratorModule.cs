using Microsoft.Extensions.DependencyInjection;
using SecitsDemoApp.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace SecitsDemoApp;

[DependsOn(
    typeof(SecitsDemoAppDbMigratorDatabaseModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule)
)]
public class SecitsDemoAppDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Configure<AbpDistributedCacheOptions>(options
        //     =>
        // {
        //     options.KeyPrefix = "SecitsDemoApp:";
        // });
        
        context.Services.AddAbpDbContext<SecitsDemoAppMigratorDbContext>(options =>
        {
            options.AddDefaultRepositories(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
        });
    }
}