using Secyud.Abp.Settings;
using Secyud.Abp.Permissions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace SecitsDemoApp;

[DependsOn(
    typeof(AbpSettingsApplicationModule),
    typeof(AbpPermissionsApplicationModule),
    typeof(SecitsDemoAppDomainModule),
    typeof(SecitsDemoAppApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class SecitsDemoAppApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<SecitsDemoAppApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<SecitsDemoAppApplicationModule>(validate: true);
        });
    }
}
