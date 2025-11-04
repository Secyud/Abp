using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Identities;

[RemoteService(Name = IdentitiesRemoteServiceConsts.RemoteServiceName)]
[Area(IdentitiesRemoteServiceConsts.ModuleName)]
[ControllerName("Settings")]
[Route("api/identities/settings")]
public class IdentitySettingsController(IIdentitySettingsAppService identitySettingsAppService) : AbpControllerBase, IIdentitySettingsAppService
{
    protected IIdentitySettingsAppService IdentitySettingsAppService { get; } = identitySettingsAppService;

    [HttpGet]
    public virtual Task<IdentitySettingsDto> GetAsync()
    {
        return IdentitySettingsAppService.GetAsync();
    }

    [HttpPut]
    public virtual Task UpdateAsync(IdentitySettingsDto input)
    {
        return IdentitySettingsAppService.UpdateAsync(input);
    }

    [HttpGet]
    [Route("ldap")]
    public virtual Task<IdentityLdapSettingsDto> GetLdapAsync()
    {
        return IdentitySettingsAppService.GetLdapAsync();
    }

    [HttpPut]
    [Route("ldap")]
    public virtual Task UpdateLdapAsync(IdentityLdapSettingsDto? input)
    {
        return IdentitySettingsAppService.UpdateLdapAsync(input);
    }

    [HttpGet]
    [Route("oauth")]
    public virtual Task<IdentityOAuthSettingsDto> GetOAuthAsync()
    {
        return IdentitySettingsAppService.GetOAuthAsync();
    }

    [HttpPut]
    [Route("oauth")]
    public virtual Task UpdateOAuthAsync(IdentityOAuthSettingsDto? input)
    {
        return IdentitySettingsAppService.UpdateOAuthAsync(input);
    }

    [HttpGet]
    [Route("session")]
    public virtual Task<IdentitySessionSettingsDto> GetSessionAsync()
    {
        return IdentitySettingsAppService.GetSessionAsync();
    }

    [HttpPut]
    [Route("session")]
    public virtual Task UpdateSessionAsync(IdentitySessionSettingsDto? input)
    {
        return IdentitySettingsAppService.UpdateSessionAsync(input);
    }
}