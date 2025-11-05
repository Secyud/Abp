using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Secyud.Abp.Account.AuthorityDelegation;
using Secyud.Abp.Account.Localization;
using Secyud.Abp.Identities;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace Secyud.Abp.Accounts;

[Authorize]
public class IdentityUserDelegationAppService : ApplicationService, IIdentityUserDelegationAppService
{
    protected IdentityUserDelegationManager IdentityUserDelegationManager { get; }
    protected IIdentityUserRepository IdentityUserRepository { get; }
    protected AbpAccountAuthorityDelegationOptions Options { get; }
    protected ILookupNormalizer LookupNormalizer { get; }

    public IdentityUserDelegationAppService(
        IdentityUserDelegationManager identityUserDelegationManager,
        IIdentityUserRepository identityUserRepository,
        IOptions<AbpAccountAuthorityDelegationOptions> options,
        ILookupNormalizer lookupNormalizer)
    {
        IdentityUserDelegationManager = identityUserDelegationManager;
        IdentityUserRepository = identityUserRepository;
        LookupNormalizer = lookupNormalizer;
        Options = options.Value;
        LocalizationResource = typeof(AbpAccountsResource);
    }

    public virtual async Task<ListResultDto<UserDelegationDto>> GetDelegatedUsersAsync()
    {
        await CheckUserDelegationOperationAsync();
        var delegations = await IdentityUserDelegationManager.GetListAsync(sourceUserId: CurrentUser.GetId());

        return await GetDelegationsAsync(
            delegations.Select(x => x.TargetUserId),
            delegations,
            false);
    }

    public virtual async Task<ListResultDto<UserDelegationDto>> GetMyDelegatedUsersAsync()
    {
        await CheckUserDelegationOperationAsync();
        var delegations = await IdentityUserDelegationManager.GetListAsync(targetUserId: CurrentUser.GetId());

        return await GetDelegationsAsync(
            delegations.Select(x => x.SourceUserId),
            delegations,
            true);
    }

    public virtual async Task<ListResultDto<UserDelegationDto>> GetActiveDelegationsAsync()
    {
        await CheckUserDelegationOperationAsync();
        var delegations = await IdentityUserDelegationManager.GetActiveDelegationsAsync(CurrentUser.GetId());
        return await GetDelegationsAsync(
            delegations.Select(x => x.SourceUserId),
            delegations,
            true);
    }

    public virtual async Task<ListResultDto<UserLookupDto>> GetUserLookupAsync(GetUserLookupInput input)
    {
        await CheckUserDelegationOperationAsync();

        var dto = new List<UserLookupDto>();
        ;
        var user = await IdentityUserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName(input.UserName));
        if (user != null)
        {
            dto.Add(ObjectMapper.Map<IdentityUser, UserLookupDto>(user));
        }

        return new ListResultDto<UserLookupDto>(dto);
    }

    protected virtual async Task<ListResultDto<UserDelegationDto>> GetDelegationsAsync(
        IEnumerable<Guid> userIds,
        List<IdentityUserDelegation> delegations,
        bool isSourceUser = true)
    {
        await CheckUserDelegationOperationAsync();
        var users = await IdentityUserRepository.GetListByIdsAsync(userIds);

        var userDelegationDto = new List<UserDelegationDto>();
        foreach (var delegation in delegations)
        {
            userDelegationDto.Add(new UserDelegationDto
            {
                Id = delegation.Id,
                UserName = users.FirstOrDefault(x => isSourceUser ? x.Id == delegation.SourceUserId : x.Id == delegation.TargetUserId)?.UserName ?? "",
                StartTime = delegation.StartTime,
                EndTime = delegation.EndTime
            });
        }

        return new ListResultDto<UserDelegationDto>(userDelegationDto);
    }

    public virtual async Task DelegateNewUserAsync(DelegateNewUserInput input)
    {
        await CheckUserDelegationOperationAsync();
        var targetUser = await IdentityUserRepository.FindAsync(input.TargetUserId);
        if (targetUser == null)
        {
            throw new UserFriendlyException(L["Secyud.Accounts:ThereIsNoUserWithId", input.TargetUserId]);
        }

        if (input.StartTime > input.EndTime)
        {
            throw new UserFriendlyException(L["Secyud.Accounts:StartTimeMustBeLessThanEndTime"]);
        }

        await IdentityUserDelegationManager.DelegateNewUserAsync(
            sourceUserId: CurrentUser.GetId(),
            targetUserId: targetUser.Id,
            startTime: input.StartTime!.Value,
            endTime: input.EndTime!.Value
        );
    }

    public virtual async Task DeleteDelegationAsync(Guid delegationId)
    {
        await CheckUserDelegationOperationAsync();
        await IdentityUserDelegationManager.DeleteDelegationAsync(delegationId, CurrentUser.GetId());
    }

    protected virtual Task CheckUserDelegationOperationAsync()
    {
        if (!Options.EnableDelegatedImpersonation)
        {
            throw new UserFriendlyException(L["Secyud.Accounts:DelegatedImpersonationIsDisabled"]);
        }

        if (CurrentUser.FindImpersonatorUserId() != null)
        {
            throw new UserFriendlyException(L["Secyud.Accounts:UserDelegationIsNotAvailableForImpersonatedUsers"]);
        }

        return Task.CompletedTask;
    }
}