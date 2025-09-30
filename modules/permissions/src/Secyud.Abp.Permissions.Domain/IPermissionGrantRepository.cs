using Volo.Abp.Domain.Repositories;

namespace Secyud.Abp.Permissions;

public interface IPermissionGrantRepository : IRepository<PermissionGrant, Guid>
{
    Task<PermissionGrant?> FindAsync(
        string name,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );

    Task<List<PermissionGrant>> GetListAsync(
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );

    Task<List<PermissionGrant>> GetListAsync(
        string[] names,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );
}
