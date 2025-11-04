using Secyud.Abp.ObjectExtending;
using Secyud.Abp.Permissions;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities;

[DependsOn(
    typeof(AbpIdentitiesDomainSharedModule),
    typeof(AbpUsersAbstractionModule),
    typeof(AbpPermissionsApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AbpIdentityApplicationContractsModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.User,
                    getApiTypes: [typeof(IdentityUserDto)],
                    createApiTypes: [typeof(IdentityUserCreateDto)],
                    updateApiTypes: [typeof(IdentityUserUpdateDto)]
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.Role,
                    getApiTypes: [typeof(IdentityRoleDto)],
                    createApiTypes: [typeof(IdentityRoleCreateDto)],
                    updateApiTypes: [typeof(IdentityRoleUpdateDto)]
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.ClaimType,
                    getApiTypes: [typeof(ClaimTypeDto)],
                    createApiTypes: [typeof(CreateClaimTypeDto)],
                    updateApiTypes: [typeof(UpdateClaimTypeDto)]
                );
        });
    }
}
