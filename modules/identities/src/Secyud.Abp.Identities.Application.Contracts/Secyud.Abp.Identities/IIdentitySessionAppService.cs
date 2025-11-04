using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Secyud.Abp.Identities;

public interface IIdentitySessionAppService : IApplicationService
{
    Task<PagedResultDto<IdentitySessionDto>> GetListAsync(GetIdentitySessionListInput input);

    Task<IdentitySessionDto> GetAsync(Guid id);

    Task RevokeAsync(Guid id);
}
