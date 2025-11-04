using Microsoft.AspNetCore.Identity;
using Secyud.Abp.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Threading;

namespace Secyud.Abp.Identities;

public class TestPermissionDataBuilder(
    IGuidGenerator guidGenerator,
    IIdentityUserRepository userRepository,
    IPermissionGrantRepository permissionGrantRepository,
    ILookupNormalizer lookupNormalizer)
    : ITransientDependency
{
    public async Task Build()
    {
        await AddRolePermissions();
        await AddUserPermissions();
    }

    private async Task AddRolePermissions()
    {
        await AddPermission(TestPermissionNames.MyPermission1, RolePermissionValueProvider.ProviderName, "moderator");
        await AddPermission(TestPermissionNames.MyPermission2, RolePermissionValueProvider.ProviderName, "moderator");

        await AddPermission(TestPermissionNames.MyPermission1, RolePermissionValueProvider.ProviderName, "supporter");
    }

    private async Task AddUserPermissions()
    {
        var david = AsyncHelper.RunSync(() => userRepository.FindByNormalizedUserNameAsync(lookupNormalizer.NormalizeName("david")));
        await AddPermission(TestPermissionNames.MyPermission1, UserPermissionValueProvider.ProviderName, david!.Id.ToString());
    }

    private async Task AddPermission(string permissionName, string providerName, string providerKey)
    {
        await permissionGrantRepository.InsertAsync(
            new PermissionGrant(
                guidGenerator.Create(),
                permissionName,
                providerName,
                providerKey
            )
        );
    }
}
