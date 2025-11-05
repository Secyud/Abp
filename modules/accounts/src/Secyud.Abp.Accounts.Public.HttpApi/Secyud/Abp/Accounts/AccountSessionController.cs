using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Secyud.Abp.Identities;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Accounts;

[RemoteService(Name = AccountProPublicRemoteServiceConsts.RemoteServiceName)]
[Area(AccountProPublicRemoteServiceConsts.ModuleName)]
[ControllerName("Sessions")]
[Route("/api/account/sessions")]
public class AccountSessionController(IAccountSessionAppService accountSessionAppService) : AbpControllerBase, IAccountSessionAppService
{
    protected IAccountSessionAppService AccountSessionAppService { get; } = accountSessionAppService;

    [HttpGet]
    public virtual Task<PagedResultDto<IdentitySessionDto>> GetListAsync(GetAccountIdentitySessionListInput input)
    {
        return AccountSessionAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<IdentitySessionDto> GetAsync(Guid id)
    {
        return AccountSessionAppService.GetAsync(id);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task RevokeAsync(Guid id)
    {
        return AccountSessionAppService.RevokeAsync(id);
    }
}
