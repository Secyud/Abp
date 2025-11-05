using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Accounts;

[RemoteService(Name = AccountsAdminRemoteServiceConsts.RemoteServiceName)]
[Area(AccountsAdminRemoteServiceConsts.ModuleName)]
[Route("api/accounts-admin/settings")]
public class AccountSettingsController(IAccountSettingsAppService accountSettingsAppService) : AbpController, IAccountSettingsAppService
{
    protected IAccountSettingsAppService AccountSettingsAppService { get; } = accountSettingsAppService;

    [HttpGet]
    public virtual async Task<AccountSettingsDto> GetAsync()
    {
        return await AccountSettingsAppService.GetAsync();
    }

    [HttpPut]
    public virtual async Task UpdateAsync(AccountSettingsDto input)
    {
        await AccountSettingsAppService.UpdateAsync(input);
    }

    [HttpGet]
    [Route("two-factor")]
    public virtual async Task<AccountTwoFactorSettingsDto> GetTwoFactorAsync()
    {
        return await AccountSettingsAppService.GetTwoFactorAsync();
    }

    [HttpPut]
    [Route("two-factor")]
    public virtual async Task UpdateTwoFactorAsync(AccountTwoFactorSettingsDto input)
    {
        await AccountSettingsAppService.UpdateTwoFactorAsync(input);
    }

    [HttpGet]
    [Route("recaptcha")]
    public virtual async Task<AccountRecaptchaSettingsDto> GetRecaptchaAsync()
    {
        return await AccountSettingsAppService.GetRecaptchaAsync();
    }

    [HttpPut]
    [Route("recaptcha")]
    public virtual async Task UpdateRecaptchaAsync(AccountRecaptchaSettingsDto input)
    {
        await AccountSettingsAppService.UpdateRecaptchaAsync(input);
    }

    [HttpGet]
    [Route("external-provider")]
    public virtual async Task<AccountExternalProviderSettingsDto> GetExternalProviderAsync()
    {
        return await AccountSettingsAppService.GetExternalProviderAsync();
    }

    [HttpPut]
    [Route("external-provider")]
    public virtual async Task UpdateExternalProviderAsync(AccountExternalProviderSettingsDto input)
    {
        await AccountSettingsAppService.UpdateExternalProviderAsync(input);
    }
}
