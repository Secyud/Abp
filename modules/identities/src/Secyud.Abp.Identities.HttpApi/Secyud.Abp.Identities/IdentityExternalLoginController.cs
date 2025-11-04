using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Identities;

[RemoteService(Name = IdentitiesRemoteServiceConsts.RemoteServiceName)]
[Area(IdentitiesRemoteServiceConsts.ModuleName)]
[ControllerName("ExternalLogin")]
[Route("api/identities/external-login")]
public class IdentityExternalLoginController(IIdentityExternalLoginAppService externalLoginAppService) : AbpControllerBase, IIdentityExternalLoginAppService
{
    public IIdentityExternalLoginAppService ExternalLoginAppService { get; } = externalLoginAppService;

    [HttpPost]
    public virtual Task CreateOrUpdateAsync()
    {
        return ExternalLoginAppService.CreateOrUpdateAsync();
    }
}