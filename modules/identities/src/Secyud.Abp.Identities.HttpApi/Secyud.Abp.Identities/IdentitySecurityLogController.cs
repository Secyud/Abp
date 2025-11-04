using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Identities;

[RemoteService(Name = IdentitiesRemoteServiceConsts.RemoteServiceName)]
[Area(IdentitiesRemoteServiceConsts.ModuleName)]
[ControllerName("SecurityLog")]
[Route("api/identities/security-logs")]
public class IdentitySecurityLogController(IIdentitySecurityLogAppService identitySecurityLogAppService) : AbpControllerBase, IIdentitySecurityLogAppService
{
    protected IIdentitySecurityLogAppService IdentitySecurityLogAppService { get; } = identitySecurityLogAppService;

    [HttpGet]
    public Task<PagedResultDto<IdentitySecurityLogDto>> GetListAsync([FromQuery] GetIdentitySecurityLogListInput input)
    {
        return IdentitySecurityLogAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public Task<IdentitySecurityLogDto> GetAsync(Guid id)
    {
        return IdentitySecurityLogAppService.GetAsync(id);
    }
}
