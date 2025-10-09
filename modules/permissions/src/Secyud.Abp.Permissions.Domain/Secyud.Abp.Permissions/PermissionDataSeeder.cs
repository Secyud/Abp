using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Permissions;

public class PermissionDataSeeder(
    IPermissionGrantRepository permissionGrantRepository,
    IGuidGenerator guidGenerator,
    ICurrentTenant currentTenant)
    : IPermissionDataSeeder, ITransientDependency
{
    protected IPermissionGrantRepository PermissionGrantRepository { get; } = permissionGrantRepository;
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;

    protected ICurrentTenant CurrentTenant { get; } = currentTenant;

    public virtual async Task SeedAsync(
        string providerName,
        string providerKey,
        IEnumerable<string> grantedPermissions,
        Guid? tenantId = null)
    {
        using (CurrentTenant.Change(tenantId))
        {
            using (PermissionGrantRepository.DisableTracking())
            {
                var names = grantedPermissions.ToArray();
                var existsPermissionGrants = (await PermissionGrantRepository.GetListAsync(names, providerName, providerKey)).Select(x => x.Name).ToList();
                var permissions = names.Except(existsPermissionGrants).Select(permissionName => new PermissionGrant(GuidGenerator.Create(), permissionName, providerName, providerKey, tenantId)).ToList();
                if (!permissions.Any())
                {
                    return;
                }
                await PermissionGrantRepository.InsertManyAsync(permissions);
            }
        }
    }
}
