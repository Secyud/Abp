using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Secyud.Abp.Identities;

public interface IIdentitySecurityLogAppService : IApplicationService
{
    Task<PagedResultDto<IdentitySecurityLogDto>> GetListAsync(GetIdentitySecurityLogListInput input);

    Task<IdentitySecurityLogDto> GetAsync(Guid id);
}
