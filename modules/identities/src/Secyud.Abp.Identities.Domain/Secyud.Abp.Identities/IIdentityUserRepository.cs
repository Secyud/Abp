using System.Security.Claims;
using Volo.Abp.Domain.Repositories;

namespace Secyud.Abp.Identities;

public interface IIdentityUserRepository : IBasicRepository<IdentityUser, Guid>
{
    Task<IdentityUser?> FindByNormalizedUserNameAsync(
        string normalizedUserName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<List<string>> GetRoleNamesAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task<IdentityUser?> FindByLoginAsync(
        string loginProvider,
        string providerKey,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<IdentityUser?> FindByNormalizedEmailAsync(
        string normalizedEmail,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityUser>> GetListByClaimAsync(
        Claim claim,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task RemoveClaimFromAllUsersAsync(
        string claimType,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
        string normalizedRoleName,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<List<Guid>> GetUserIdListByRoleIdAsync(
        Guid roleId,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityUser>> GetListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false,
        Guid? roleId = null,
        Guid? id = null,
        string? userName = null,
        string? phoneNumber = null,
        string? emailAddress = null,
        string? name = null,
        string? surname = null,
        bool? isLockedOut = null,
        bool? notActive = null,
        bool? emailConfirmed = null,
        bool? isExternal = null,
        DateTime? maxCreationTime = null,
        DateTime? minCreationTime = null,
        DateTime? maxModifitionTime = null,
        DateTime? minModifitionTime = null,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityRole>> GetRolesAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string? filter = null,
        Guid? roleId = null,
        Guid? id = null,
        string? userName = null,
        string? phoneNumber = null,
        string? emailAddress = null,
        string? name = null,
        string? surname = null,
        bool? isLockedOut = null,
        bool? notActive = null,
        bool? emailConfirmed = null,
        bool? isExternal = null,
        DateTime? maxCreationTime = null,
        DateTime? minCreationTime = null,
        DateTime? maxModifitionTime = null,
        DateTime? minModifitionTime = null,
        CancellationToken cancellationToken = default
    );

    Task<IdentityUser?> FindByTenantIdAndUserNameAsync(
        string userName,
        Guid? tenantId,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityUser>> GetListByIdsAsync(
        IEnumerable<Guid> ids,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task UpdateRoleAsync(
        Guid sourceRoleId,
        Guid? targetRoleId,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityUserIdWithRoleNames>> GetRoleNamesAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default);
}
