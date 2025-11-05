using Microsoft.AspNetCore.Mvc;
using Secyud.Abp.Accounts.ExternalProviders;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Accounts;

[RemoteService(Name = AccountProPublicRemoteServiceConsts.RemoteServiceName)]
[Area(AccountProPublicRemoteServiceConsts.ModuleName)]
[Route("api/account/external-provider")]
public class AccountExternalProviderController(IAccountExternalProviderAppService accountExternalProviderAppService)
    : AbpControllerBase, IAccountExternalProviderAppService
{
    protected IAccountExternalProviderAppService AccountExternalProviderAppService { get; } = accountExternalProviderAppService;

    [HttpGet]
    public virtual async Task<ExternalProviderDto> GetAllAsync()
    {
        return await AccountExternalProviderAppService.GetAllAsync();
    }

    [HttpGet]
    [Route("by-name")]
    public virtual async Task<ExternalProviderItemWithSecretDto> GetByNameAsync(GetByNameInput input)
    {
        return await AccountExternalProviderAppService.GetByNameAsync(input);
    }
}
