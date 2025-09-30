using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Permissions;

[DependsOn(
    typeof(AbpPermissionsDomainModule),
    typeof(AbpPermissionsApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpDddApplicationModule)
    )]
public class AbpPermissionsApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpPermissionsAutoMapperProfile>();
        });
    }
}
