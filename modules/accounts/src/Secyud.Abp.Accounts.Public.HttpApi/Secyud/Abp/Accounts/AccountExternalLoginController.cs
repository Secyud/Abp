using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Accounts;

[RemoteService(Name = AccountProPublicRemoteServiceConsts.RemoteServiceName)]
[Area(AccountProPublicRemoteServiceConsts.ModuleName)]
[ControllerName("ExternalLogin")]
[Route("/api/account/externallogin")]
public class AccountExternalLoginController : AbpControllerBase, IAccountExternalLoginAppService
{
    protected IAccountExternalLoginAppService AccountExternalLoginAppService { get; }

    public AccountExternalLoginController(IAccountExternalLoginAppService accountExternalLoginAppService)
    {
        AccountExternalLoginAppService = accountExternalLoginAppService;
    }

    [HttpGet]
    public virtual Task<List<AccountExternalLoginDto>> GetListAsync()
    {
        return AccountExternalLoginAppService.GetListAsync();
    }

    [HttpDelete]
    [Route("{loginProvider}/{providerKey}")]
    public virtual Task DeleteAsync(string loginProvider, string providerKey)
    {
        return AccountExternalLoginAppService.DeleteAsync(loginProvider, providerKey);
    }

    [HttpGet]
    [Route("password-verified/{userId}/{loginProvider}/{providerKey}")]
    public virtual Task<bool> HasPasswordVerifiedAsync(Guid userId, string loginProvider, string providerKey)
    {
        return AccountExternalLoginAppService.HasPasswordVerifiedAsync(userId, loginProvider, providerKey);
    }

    [HttpPost]
    [Route("password-verified/{loginProvider}/{providerKey}")]
    public virtual Task SetPasswordVerifiedAsync(string loginProvider, string providerKey)
    {
        return AccountExternalLoginAppService.SetPasswordVerifiedAsync(loginProvider, providerKey);
    }

    [HttpDelete]
    [Route("{loginProvider}/{providerKey}/password-verified")]
    public virtual Task RemovePasswordVerifiedAsync(string loginProvider, string providerKey)
    {
        return AccountExternalLoginAppService.RemovePasswordVerifiedAsync(loginProvider, providerKey);
    }
}
