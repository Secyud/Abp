using Secyud.Abp.Permissions;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using SecitsDemoApp.Localization;
using Volo.Abp.Domain;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SecitsDemoApp;

[DependsOn(
    typeof(AbpPermissionsDomainSharedModule),
    typeof(AbpValidationModule),
    typeof(AbpDddDomainSharedModule)
)]
public class SecitsDemoAppDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<SecitsDemoAppDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<SecitsDemoAppResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/SecitsDemoApp");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("SecitsDemoApp", typeof(SecitsDemoAppResource));
        });
    }
}
