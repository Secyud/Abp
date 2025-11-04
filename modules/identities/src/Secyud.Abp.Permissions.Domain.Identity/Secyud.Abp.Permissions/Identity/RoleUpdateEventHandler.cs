using Secyud.Abp.Identities;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Secyud.Abp.Permissions.Identity;

public class RoleUpdateEventHandler(
    IPermissionManager permissionManager,
    IPermissionGrantRepository permissionGrantRepository)
    :
        IDistributedEventHandler<IdentityRoleNameChangedEto>,
        ITransientDependency
{
    protected IPermissionManager PermissionManager { get; } = permissionManager;
    protected IPermissionGrantRepository PermissionGrantRepository { get; } = permissionGrantRepository;

    public async Task HandleEventAsync(IdentityRoleNameChangedEto eventData)
    {
        if (eventData.OldName is null)
        {
            return;
        }

        var permissionGrantsInRole = await PermissionGrantRepository.GetListAsync(RolePermissionValueProvider.ProviderName, eventData.OldName);

        await PermissionManager.DeleteAsync(RolePermissionValueProvider.ProviderName, eventData.OldName);

        if (eventData.Name is not null)
        {
            await PermissionManager.UpdateAsync(RolePermissionValueProvider.ProviderName, eventData.Name,
                permissionGrantsInRole.Select(u => u.Name).ToArray(), []);
        }
    }
}