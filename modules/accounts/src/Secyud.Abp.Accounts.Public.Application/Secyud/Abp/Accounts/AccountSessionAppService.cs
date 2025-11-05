using Microsoft.AspNetCore.Authorization;
using Secyud.Abp.Identities;
using Secyud.Abp.Identities.Session;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace Secyud.Abp.Accounts;

[Authorize]
public class AccountSessionAppService(
    IdentitySessionManager identitySessionManager,
    IIdentitySessionRepository identitySessionRepository,
    IIdentityUserRepository identityUserRepository)
    : ApplicationService, IAccountSessionAppService
{
    protected IdentitySessionManager IdentitySessionManager { get; } = identitySessionManager;
    protected IIdentitySessionRepository IdentitySessionRepository { get; } = identitySessionRepository;
    protected IIdentityUserRepository IdentityUserRepository { get; } = identityUserRepository;

    public virtual async Task<PagedResultDto<IdentitySessionDto>> GetListAsync(GetAccountIdentitySessionListInput input)
    {
        var count = await IdentitySessionRepository.GetCountAsync(
            CurrentUser.GetId(),
            input.Device,
            input.ClientId
        );

        var sessions = await IdentitySessionManager.GetListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            CurrentUser.GetId(),
            input.Device,
            input.ClientId);

        var dtos = new List<IdentitySessionDto>(ObjectMapper.Map<List<IdentitySession>, List<IdentitySessionDto>>(sessions));
        foreach (var dto in dtos)
        {
            dto.IsCurrent = dto.SessionId == CurrentUser.GetSessionId();
            dto.TenantName = CurrentTenant.Name;
            dto.UserName = CurrentUser.UserName;
        }

        return new PagedResultDto<IdentitySessionDto>(count, dtos);
    }

    public virtual async Task<IdentitySessionDto> GetAsync(Guid id)
    {
        var session = await GetCurrentUserSessionAsync(id);
        var dto = ObjectMapper.Map<IdentitySession, IdentitySessionDto>(session);

        dto.IsCurrent = dto.SessionId == CurrentUser.GetSessionId();
        if (dto.TenantId.HasValue)
        {
            dto.TenantName = CurrentTenant.Name;
        }
        dto.UserName = CurrentUser.Name;
        return dto;
    }

    public virtual async Task RevokeAsync(Guid id)
    {
        var session = await GetCurrentUserSessionAsync(id);
        await IdentitySessionManager.RevokeAsync(session.Id);
    }

    protected virtual async Task<IdentitySession> GetCurrentUserSessionAsync(Guid id)
    {
        var session = await IdentitySessionManager.GetAsync(id);
        if (session.UserId != CurrentUser.GetId())
        {
            throw new EntityNotFoundException(typeof(IdentitySession), id);
        }
        return session;
    }
}
