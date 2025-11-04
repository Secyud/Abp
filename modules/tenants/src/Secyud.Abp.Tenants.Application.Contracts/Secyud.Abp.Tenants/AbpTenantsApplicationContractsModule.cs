using Secyud.Abp.ObjectExtending;
using Secyud.Abp.Permissions;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace Secyud.Abp.Tenants;

[DependsOn(
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpTenantsDomainSharedModule),
    typeof(AbpPermissionsModule)
    )]
public class AbpTenantsApplicationContractsModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    TenantsModuleExtensionConsts.ModuleName,
                    TenantsModuleExtensionConsts.EntityNames.Tenant,
                    getApiTypes: new[] { typeof(TenantDto) },
                    createApiTypes: new[] { typeof(TenantCreateDto) },
                    updateApiTypes: new[] { typeof(TenantUpdateDto) }
                );
        });
    }
}
