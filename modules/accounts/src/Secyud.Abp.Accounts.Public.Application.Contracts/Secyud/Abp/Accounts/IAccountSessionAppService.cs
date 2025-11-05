using Secyud.Abp.Identities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Secyud.Abp.Accounts;

public interface IAccountSessionAppService : IApplicationService
{
    Task<PagedResultDto<IdentitySessionDto>> GetListAsync(GetAccountIdentitySessionListInput input);

    Task<IdentitySessionDto> GetAsync(Guid id);

    Task RevokeAsync(Guid id);
}
