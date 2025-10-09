using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Secyud.Abp.Permissions;

public class PermissionTestDataBuilder(IGuidGenerator guidGenerator, IPermissionGrantRepository permissionGrantRepository)
    : ITransientDependency
{
    public static Guid User1Id { get; } = Guid.NewGuid();
    public static Guid User2Id { get; } = Guid.NewGuid();

    public async Task BuildAsync()
    {
        await permissionGrantRepository.InsertAsync(
            new PermissionGrant(
                guidGenerator.Create(),
                "MyPermission1",
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );

        await permissionGrantRepository.InsertAsync(
            new PermissionGrant(
                guidGenerator.Create(),
                "MyDisabledPermission1",
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );

        await permissionGrantRepository.InsertAsync(
            new PermissionGrant(
                guidGenerator.Create(),
                "MyPermission3",
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );

        await permissionGrantRepository.InsertAsync(
            new PermissionGrant(
                guidGenerator.Create(),
                "MyPermission5",
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );
    }
}
