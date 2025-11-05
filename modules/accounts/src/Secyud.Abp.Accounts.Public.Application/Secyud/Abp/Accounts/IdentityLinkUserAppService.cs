using Microsoft.AspNetCore.Authorization;
using Secyud.Abp.Identities;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace Secyud.Abp.Accounts;

[Authorize]
public class IdentityLinkUserAppService(
    IdentityLinkUserManager identityLinkUserManager,
    IIdentityUserRepository identityUserRepository,
    ITenantStore tenantStore)
    : ApplicationService, IIdentityLinkUserAppService
{
    protected IdentityLinkUserManager IdentityLinkUserManager { get; } = identityLinkUserManager;
    protected IIdentityUserRepository IdentityUserRepository { get; } = identityUserRepository;
    protected ITenantStore TenantStore { get; } = tenantStore;

    public virtual async Task<ListResultDto<LinkUserDto>> GetAllListAsync()
    {
        var currentUserId = CurrentUser.GetId();
        var currentTenantId = CurrentTenant.Id;
        using (CurrentTenant.Change(null))
        {
            var linkUsers = await IdentityLinkUserManager.GetListAsync(new IdentityLinkUserInfo(currentUserId, currentTenantId), includeIndirect: true);

            var allLinkUsers = linkUsers.Select(x => new LinkUserDto
            {
                TargetTenantId = x.TargetTenantId,
                TargetUserId = x.TargetUserId,
                DirectlyLinked = x.SourceTenantId == currentTenantId && x.SourceUserId == currentUserId || x.TargetTenantId == currentTenantId && x.TargetUserId == currentUserId
            }).Concat(linkUsers.Select(x => new LinkUserDto
            {
                TargetTenantId = x.SourceTenantId,
                TargetUserId = x.SourceUserId,
                DirectlyLinked = x.SourceTenantId == currentTenantId && x.SourceUserId == currentUserId || x.TargetTenantId == currentTenantId && x.TargetUserId == currentUserId
            })).GroupBy(x => new { x.TargetTenantId, x.TargetUserId })
                .Select(x => x.OrderByDescending(y => y.DirectlyLinked).First())
                .Where(x => x.TargetTenantId != currentTenantId || x.TargetUserId != currentUserId)
                .ToList();

            var userDto = new List<LinkUserDto>();

            foreach (var userGroup in allLinkUsers.GroupBy(x => x.TargetTenantId))
            {
                var tenantId = userGroup.Key;

                TenantConfiguration? tenant = null;
                if (tenantId.HasValue)
                {
                    tenant = await TenantStore.FindAsync(tenantId.Value);
                }

                using (CurrentTenant.Change(tenantId))
                {
                    var users = await IdentityUserRepository.GetListByIdsAsync(userGroup.Select(x => x.TargetUserId));
                    foreach (var user in users)
                    {
                        userDto.Add(new LinkUserDto
                        {
                            TargetUserId = user.Id,
                            TargetUserName = user.UserName,
                            TargetTenantId = tenant?.Id,
                            TargetTenantName = tenant?.Name,
                            DirectlyLinked = userGroup.FirstOrDefault(x => x.TargetUserId == user.Id)?.DirectlyLinked ?? false
                        });
                    }
                }
            }

            return new ListResultDto<LinkUserDto>(userDto);
        }
    }

    public virtual async Task LinkAsync(LinkUserInput input)
    {
        if (await IdentityLinkUserManager.VerifyLinkTokenAsync(new IdentityLinkUserInfo(input.UserId, input.TenantId), input.Token, LinkUserTokenProviderConsts.LinkUserTokenPurpose))
        {
            await IdentityLinkUserManager.LinkAsync(new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentTenant.Id),
                new IdentityLinkUserInfo(input.UserId, input.TenantId));
        }
        else
        {
            throw new UserFriendlyException("InvalidLinkToken");
        }
    }

    public virtual async Task UnlinkAsync(UnLinkUserInput input)
    {
        await IdentityLinkUserManager.UnlinkAsync(new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentTenant.Id),
            new IdentityLinkUserInfo(input.UserId, input.TenantId));
    }

    public virtual async Task<bool> IsLinkedAsync(IsLinkedInput input)
    {
        return await IdentityLinkUserManager.IsLinkedAsync(
            new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentTenant.Id),
            new IdentityLinkUserInfo(input.UserId, input.TenantId),
            true);
    }

    public virtual async Task<string> GenerateLinkTokenAsync()
    {
        return await IdentityLinkUserManager.GenerateLinkTokenAsync(
            new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentTenant.Id),
            LinkUserTokenProviderConsts.LinkUserTokenPurpose);
    }

    [AllowAnonymous]
    public virtual async Task<bool> VerifyLinkTokenAsync(VerifyLinkTokenInput input)
    {
        return await IdentityLinkUserManager.VerifyLinkTokenAsync(
            new IdentityLinkUserInfo(input.UserId, input.TenantId),
            input.Token,
            LinkUserTokenProviderConsts.LinkUserTokenPurpose);
    }

    public virtual async Task<string> GenerateLinkLoginTokenAsync()
    {
        return await IdentityLinkUserManager.GenerateLinkTokenAsync(
            new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentTenant.Id),
            LinkUserTokenProviderConsts.LinkUserLoginTokenPurpose);
    }

    [AllowAnonymous]
    public virtual async Task<bool> VerifyLinkLoginTokenAsync(VerifyLinkLoginTokenInput input)
    {
        return await IdentityLinkUserManager.VerifyLinkTokenAsync(
            new IdentityLinkUserInfo(input.UserId, input.TenantId),
            input.Token,
            LinkUserTokenProviderConsts.LinkUserLoginTokenPurpose);
    }
}
