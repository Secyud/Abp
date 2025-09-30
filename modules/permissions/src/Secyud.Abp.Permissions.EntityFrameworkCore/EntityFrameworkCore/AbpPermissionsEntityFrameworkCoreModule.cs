using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Permissions.EntityFrameworkCore;

[DependsOn(typeof(AbpPermissionsDomainModule))]
[DependsOn(typeof(AbpEntityFrameworkCoreModule))]
public class AbpPermissionsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<PermissionsDbContext>(options =>
        {
            options.AddDefaultRepositories<IPermissionsDbContext>();

            options.AddRepository<PermissionGroupDefinitionRecord, EfCorePermissionGroupDefinitionRecordRepository>();
            options.AddRepository<PermissionDefinitionRecord, EfCorePermissionDefinitionRecordRepository>();
            options.AddRepository<PermissionGrant, EfCorePermissionGrantRepository>();
        });
    }
}
