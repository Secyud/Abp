using Secyud.Abp.Identities.Localization;
using Volo.Abp.Auditing;
using Volo.Abp.Features;
using Volo.Abp.Ldap;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Identities;

[DependsOn(
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpLocalizationModule),
    typeof(AbpFeaturesModule),
    typeof(AbpAuditingContractsModule),
    typeof(AbpLdapAbstractionsModule),
    typeof(AbpValidationModule),
    typeof(AbpSettingsModule)
)]
public class AbpIdentitiesDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options
            =>
        {
            options.FileSets.AddEmbedded<AbpIdentitiesDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpIdentitiesResource>()
                .AddVirtualJson("Secyud/Abp/Identities/Localization/DomainShared");
        });

        Configure<AbpExceptionLocalizationOptions>(options
            =>
        {
            options.MapCodeNamespace("Secyud.Abp.Identities", typeof(AbpIdentitiesResource));
        });
    }
}