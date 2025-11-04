using System.Linq.Dynamic.Core;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Identities.EntityFrameworkCore;

public class EfCoreIdentityUserRepository(IDbContextProvider<IIdentitiesDbContext> dbContextProvider)
    : EfCoreRepository<IIdentitiesDbContext, IdentityUser, Guid>(dbContextProvider), IIdentityUserRepository
{
    public virtual async Task<IdentityUser?> FindByNormalizedUserNameAsync(
        string normalizedUserName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(
                u => u.NormalizedUserName == normalizedUserName,
                GetCancellationToken(cancellationToken)
            );
    }

    public virtual async Task<List<string>> GetRoleNamesAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query =
            from userRole in dbContext.Set<IdentityUserRole>()
            join role in dbContext.Roles on userRole.RoleId equals role.Id
            where userRole.UserId == id
            select role.Name;

        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityUserIdWithRoleNames>> GetRoleNamesAsync(
        IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var userRoles = await (
            from userRole in dbContext.Set<IdentityUserRole>()
            join role in dbContext.Roles on userRole.RoleId equals role.Id
            where userIds.Contains(userRole.UserId)
            group new
            {
                userRole.UserId,
                role.Name
            } by userRole.UserId
            into gp
            select new IdentityUserIdWithRoleNames
            {
                Id = gp.Key,
                RoleNames = gp.Select(x => x.Name).ToArray()
            }).ToListAsync(cancellationToken: cancellationToken);


        return userRoles;
    }

    public virtual async Task<IdentityUser?> FindByLoginAsync(
        string loginProvider,
        string providerKey,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(u => u.Logins.Any(login => login.LoginProvider == loginProvider && login.ProviderKey == providerKey))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<IdentityUser?> FindByNormalizedEmailAsync(
        string normalizedEmail,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityUser>> GetListByClaimAsync(
        Claim claim,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(u => u.Claims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task RemoveClaimFromAllUsersAsync(string claimType, bool autoSave, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var userClaims = await dbContext.Set<IdentityUserClaim>().Where(uc => uc.ClaimType == claimType).ToListAsync(cancellationToken: cancellationToken);
        if (userClaims.Count != 0)
        {
            (await GetDbContextAsync()).Set<IdentityUserClaim>().RemoveRange(userClaims);
            if (autoSave)
            {
                await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }
    }

    public virtual async Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
        string normalizedRoleName,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var role = await dbContext.Roles
            .Where(x => x.NormalizedName == normalizedRoleName)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

        if (role == null)
        {
            return [];
        }

        return await dbContext.Users
            .IncludeDetails(includeDetails)
            .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Guid>> GetUserIdListByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbContextAsync()).Set<IdentityUserRole>().Where(x => x.RoleId == roleId)
            .Select(x => x.UserId).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityUser>> GetListAsync(
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
        CancellationToken cancellationToken = default)
    {
        var query = await GetFilteredQueryableAsync(
            filter, roleId, id, userName, phoneNumber, emailAddress,
            name, surname, isLockedOut, notActive, emailConfirmed, isExternal, maxCreationTime,
            minCreationTime, maxModifitionTime, minModifitionTime, cancellationToken
        );

        return await query.IncludeDetails(includeDetails)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(IdentityUser.CreationTime) + " desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityRole>> GetRolesAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var query = from userRole in dbContext.Set<IdentityUserRole>()
            join role in dbContext.Roles.IncludeDetails(includeDetails) on userRole.RoleId equals role.Id
            where userRole.UserId == id
            select role;

        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetCountAsync(
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
        CancellationToken cancellationToken = default)
    {
        return await (await GetFilteredQueryableAsync(
            filter, roleId, id, userName, phoneNumber, emailAddress, name,
            surname, isLockedOut, notActive, emailConfirmed, isExternal,
            maxCreationTime, minCreationTime, maxModifitionTime, minModifitionTime, cancellationToken
        )).LongCountAsync(GetCancellationToken(cancellationToken));
    }

    [Obsolete("Use WithDetailsAsync method.")]
    public override IQueryable<IdentityUser> WithDetails()
    {
        return GetQueryable().IncludeDetails();
    }

    public override async Task<IQueryable<IdentityUser>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }

    public virtual async Task<IdentityUser?> FindByTenantIdAndUserNameAsync(
        string userName,
        Guid? tenantId,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .FirstOrDefaultAsync(u => u.TenantId == tenantId && u.UserName == userName, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityUser>> GetListByIdsAsync(IEnumerable<Guid> ids, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task UpdateRoleAsync(Guid sourceRoleId, Guid? targetRoleId, CancellationToken cancellationToken = default)
    {
        if (targetRoleId != null)
        {
            var users = await (await GetDbContextAsync()).Set<IdentityUserRole>().Where(x => x.RoleId == targetRoleId).Select(x => x.UserId)
                .ToArrayAsync(cancellationToken: cancellationToken);
            await (await GetDbContextAsync()).Set<IdentityUserRole>().Where(x => x.RoleId == sourceRoleId && !users.Contains(x.UserId))
                .ExecuteUpdateAsync(t => t.SetProperty(e => e.RoleId, targetRoleId), GetCancellationToken(cancellationToken));
            await (await GetDbContextAsync()).Set<IdentityUserRole>().Where(x => x.RoleId == sourceRoleId)
                .ExecuteDeleteAsync(GetCancellationToken(cancellationToken));
        }
        else
        {
            await (await GetDbContextAsync()).Set<IdentityUserRole>().Where(x => x.RoleId == sourceRoleId)
                .ExecuteDeleteAsync(GetCancellationToken(cancellationToken));
        }
    }

    protected virtual async Task<IQueryable<IdentityUser>> GetFilteredQueryableAsync(
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
        CancellationToken cancellationToken = default)
    {
        var upperFilter = filter?.ToUpperInvariant();
        var query = await GetQueryableAsync();

        if (id.HasValue)
        {
            return query.Where(x => x.Id == id);
        }

        if (roleId.HasValue)
        {
            query = query.Where(identityUser => identityUser.Roles.Any(x => x.RoleId == roleId.Value));
        }

        return query
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.NormalizedUserName!.Contains(upperFilter!) ||
                    u.NormalizedEmail!.Contains(upperFilter!) ||
                    u.Name!.Contains(filter!) ||
                    u.Surname!.Contains(filter!) ||
                    u.PhoneNumber!.Contains(filter!)
            )
            .WhereIf(!string.IsNullOrWhiteSpace(userName), x => x.UserName == userName)
            .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), x => x.PhoneNumber == phoneNumber)
            .WhereIf(!string.IsNullOrWhiteSpace(emailAddress), x => x.Email == emailAddress)
            .WhereIf(!string.IsNullOrWhiteSpace(name), x => x.Name == name)
            .WhereIf(!string.IsNullOrWhiteSpace(surname), x => x.Surname == surname)
            .WhereIf(isLockedOut.HasValue,
                x => (x.LockoutEnabled && x.LockoutEnd.HasValue && x.LockoutEnd.Value.CompareTo(DateTime.UtcNow) > 0) == isLockedOut!.Value)
            .WhereIf(notActive.HasValue, x => x.IsActive == !notActive!.Value)
            .WhereIf(emailConfirmed.HasValue, x => x.EmailConfirmed == emailConfirmed!.Value)
            .WhereIf(isExternal.HasValue, x => x.IsExternal == isExternal!.Value)
            .WhereIf(maxCreationTime != null, p => p.CreationTime <= maxCreationTime)
            .WhereIf(minCreationTime != null, p => p.CreationTime >= minCreationTime)
            .WhereIf(maxModifitionTime != null, p => p.LastModificationTime <= maxModifitionTime)
            .WhereIf(minModifitionTime != null, p => p.LastModificationTime >= minModifitionTime);
    }
}