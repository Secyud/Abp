using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Tenants;

[DependsOn(typeof(AbpTenantsDomainModule))]
[DependsOn(typeof(AbpTenantsApplicationContractsModule))]
[DependsOn(typeof(AbpDddApplicationModule))]
public class AbpTenantsApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpTenantsApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpTenantsApplicationAutoMapperProfile>(validate: true);
        });
    }
}
