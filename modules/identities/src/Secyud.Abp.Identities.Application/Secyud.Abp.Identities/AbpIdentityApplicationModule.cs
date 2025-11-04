using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Permissions;
using Secyud.Abp.Settings;
using Volo.Abp.AutoMapper;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Identities;

[DependsOn(
    typeof(AbpIdentitiesDomainModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpEventBusModule),
    typeof(AbpPermissionsApplicationModule),
    typeof(AbpSettingsDomainModule)
    )]
public class AbpIdentityApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpIdentityApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpIdentityApplicationModuleAutoMapperProfile>();
        });
    }
}
