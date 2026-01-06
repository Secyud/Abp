using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Secyud.Abp.Permissions;

public interface IPermissionAppService : IApplicationService
{
    Task<PagedResultDto<PermissionGrantInfoDto>> GetListAsync(PermissionGrantInfoRequestInput input);

    Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input);
}