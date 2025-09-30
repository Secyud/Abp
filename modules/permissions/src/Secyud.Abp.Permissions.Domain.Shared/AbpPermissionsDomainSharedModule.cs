using Secyud.Abp.Permissions.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Permissions;

[DependsOn(
    typeof(AbpValidationModule)
)]
public class AbpPermissionsDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<AbpPermissionsDomainSharedModule>(); });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpPermissionsResource>("en")
                .AddBaseTypes(
                    typeof(AbpValidationResource)
                ).AddVirtualJson("/Localization/Permissions");
        });

        Configure<AbpExceptionLocalizationOptions>(options
            =>
        {
            options.MapCodeNamespace("Secyud.Abp.Permissions", typeof(AbpPermissionsResource));
        });
    }
}