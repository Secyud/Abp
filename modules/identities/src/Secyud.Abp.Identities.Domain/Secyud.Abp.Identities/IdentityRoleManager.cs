using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Secyud.Abp.Identities.Localization;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.Security.Claims;
using Volo.Abp.Threading;

namespace Secyud.Abp.Identities;

public class IdentityRoleManager(
    IdentityRoleStore store,
    IEnumerable<IRoleValidator<IdentityRole>> roleValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    ILogger<IdentityRoleManager> logger,
    IStringLocalizer<AbpIdentitiesResource> localizer,
    ICancellationTokenProvider cancellationTokenProvider,
    IIdentityUserRepository userRepository,
    IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache)
    : RoleManager<IdentityRole>(store, roleValidators, keyNormalizer, errors, logger), IDomainService
{
    protected override CancellationToken CancellationToken => CancellationTokenProvider.Token;

    protected IStringLocalizer<AbpIdentitiesResource> Localizer { get; } = localizer;
    protected ICancellationTokenProvider CancellationTokenProvider { get; } = cancellationTokenProvider;
    protected IIdentityUserRepository UserRepository { get; } = userRepository;
    protected IDistributedCache<AbpDynamicClaimCacheItem> DynamicClaimCache { get; } = dynamicClaimCache;

    public virtual async Task<IdentityRole> GetByIdAsync(Guid id)
    {
        var role = await Store.FindByIdAsync(id.ToString(), CancellationToken);
        if (role == null)
        {
            throw new EntityNotFoundException(typeof(IdentityRole), id);
        }

        return role;
    }

    public override async Task<IdentityResult> SetRoleNameAsync(IdentityRole role, string? name)
    {
        if (role.IsStatic && role.Name != name)
        {
            throw new BusinessException(IdentitiesErrorCodes.StaticRoleRenaming);
        }

        var userIdList = await UserRepository.GetUserIdListByRoleIdAsync(role.Id, cancellationToken: CancellationToken);
        var result = await base.SetRoleNameAsync(role, name);
        if (result.Succeeded)
        {
            Logger.LogDebug("Remove dynamic claims cache for users of role: {RoleId}", role.Id);
            await DynamicClaimCache.RemoveManyAsync(userIdList.Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, role.TenantId)),
                token: CancellationToken);
        }

        return result;
    }

    public override async Task<IdentityResult> DeleteAsync(IdentityRole role)
    {
        if (role.IsStatic)
        {
            throw new BusinessException(IdentitiesErrorCodes.StaticRoleDeletion);
        }

        var userIdList = await UserRepository.GetUserIdListByRoleIdAsync(role.Id, cancellationToken: CancellationToken);
        var result = await base.DeleteAsync(role);
        if (result.Succeeded)
        {
            Logger.LogDebug("Remove dynamic claims cache for users of role: {RoleId}", role.Id);
            await DynamicClaimCache.RemoveManyAsync(userIdList
                    .Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, role.TenantId)),
                token: CancellationToken);
        }

        return result;
    }
}