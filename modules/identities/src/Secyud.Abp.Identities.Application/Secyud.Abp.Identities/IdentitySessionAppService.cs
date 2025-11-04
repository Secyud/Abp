using Microsoft.AspNetCore.Authorization;
using Secyud.Abp.Identities.Session;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities;

[Authorize(IdentityPermissions.Sessions.DefaultName)]
public class IdentitySessionAppService(
    IdentitySessionManager identitySessionManager,
    IIdentitySessionRepository identitySessionRepository,
    IIdentityUserRepository identityUserRepository)
    : IdentityAppServiceBase, IIdentitySessionAppService
{
    protected IdentitySessionManager IdentitySessionManager { get; } = identitySessionManager;
    protected IIdentitySessionRepository IdentitySessionRepository { get; } = identitySessionRepository;
    protected IIdentityUserRepository IdentityUserRepository { get; } = identityUserRepository;

    public virtual async Task<PagedResultDto<IdentitySessionDto>> GetListAsync(GetIdentitySessionListInput input)
    {
        var count = await IdentitySessionRepository.GetCountAsync(
            input.UserId,
            input.Device,
            input.ClientId
        );

        var sessions = await  IdentitySessionManager.GetListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.UserId,
            input.Device,
            input.ClientId
        );

        var dtos = new List<IdentitySessionDto>(ObjectMapper.Map<List<IdentitySession>, List<IdentitySessionDto>>(sessions));
        var users = await IdentityUserRepository.GetListByIdsAsync(dtos.Select(x => x.UserId).ToArray());
        foreach (var dto in dtos)
        {
            dto.IsCurrent = dto.SessionId == CurrentUser.GetSessionId();
            if (dto.TenantId.HasValue)
            {
                dto.TenantName = CurrentTenant.Name;
            }

            dto.UserName = users.FirstOrDefault(x => x.Id == dto.UserId)?.UserName;
        }

        return new PagedResultDto<IdentitySessionDto>(count, dtos);
    }

    public virtual async Task<IdentitySessionDto> GetAsync(Guid id)
    {
        var session = await IdentitySessionManager.GetAsync(id);
        var dto = ObjectMapper.Map<IdentitySession, IdentitySessionDto>(session);
        if (dto.TenantId.HasValue)
        {
            dto.TenantName = CurrentTenant.Name;
        }
        var user = await IdentityUserRepository.GetAsync(dto.UserId);
        dto.UserName = user.UserName;
        return dto;
    }

    public virtual async Task RevokeAsync(Guid id)
    {
        await IdentitySessionManager.RevokeAsync(id);
    }
}
