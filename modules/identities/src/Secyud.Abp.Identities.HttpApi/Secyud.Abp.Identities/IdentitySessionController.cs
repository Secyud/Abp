using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Identities;

[RemoteService(Name = IdentitiesRemoteServiceConsts.RemoteServiceName)]
[Area(IdentitiesRemoteServiceConsts.ModuleName)]
[ControllerName("Sessions")]
[Route("/api/identities/sessions")]
public class IdentitySessionController(IIdentitySessionAppService identitySessionAppService) : AbpControllerBase, IIdentitySessionAppService
{
    protected IIdentitySessionAppService IdentitySessionAppService { get; } = identitySessionAppService;

    [HttpGet]
    public virtual Task<PagedResultDto<IdentitySessionDto>> GetListAsync(GetIdentitySessionListInput input)
    {
        return IdentitySessionAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<IdentitySessionDto> GetAsync(Guid id)
    {
        return IdentitySessionAppService.GetAsync(id);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task RevokeAsync(Guid id)
    {
        return IdentitySessionAppService.RevokeAsync(id);
    }
}
