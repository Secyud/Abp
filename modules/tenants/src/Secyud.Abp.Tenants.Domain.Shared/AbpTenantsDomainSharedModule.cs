using Secyud.Abp.Tenants.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Tenants;

[DependsOn(typeof(AbpValidationModule))]
public class AbpTenantsDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<AbpTenantsDomainSharedModule>(); });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpTenantsResource>("en")
                .AddBaseTypes(
                    typeof(AbpValidationResource)
                ).AddVirtualJson("/Localization/Tenants");
        });

        Configure<AbpExceptionLocalizationOptions>(options => { options.MapCodeNamespace("Secyud.Abp.Tenants", typeof(AbpTenantsResource)); });
    }
}