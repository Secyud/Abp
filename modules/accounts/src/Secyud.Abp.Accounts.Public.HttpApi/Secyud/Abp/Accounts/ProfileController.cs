using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Accounts;

[RemoteService(Name = AccountProPublicRemoteServiceConsts.RemoteServiceName)]
[Area(AccountProPublicRemoteServiceConsts.ModuleName)]
[ControllerName("Profile")]
[Route("/api/account/my-profile")]
public class ProfileController : AbpControllerBase, IProfileAppService
{
    protected IProfileAppService ProfileAppService { get; }

    public ProfileController(IProfileAppService profileAppService)
    {
        ProfileAppService = profileAppService;
    }

    [HttpGet]
    public virtual Task<ProfileDto> GetAsync()
    {
        return ProfileAppService.GetAsync();
    }

    [HttpPut]
    public virtual Task<ProfileDto> UpdateAsync(UpdateProfileDto input)
    {
        return ProfileAppService.UpdateAsync(input);
    }

    [HttpPost]
    [Route("change-password")]
    public virtual Task ChangePasswordAsync(ChangePasswordInput input)
    {
        return ProfileAppService.ChangePasswordAsync(input);
    }

    [HttpGet]
    [Route("two-factor-enabled")]
    public virtual Task<bool> GetTwoFactorEnabledAsync()
    {
        return ProfileAppService.GetTwoFactorEnabledAsync();
    }

    [HttpPost]
    [Route("set-two-factor-enabled")]
    public virtual Task SetTwoFactorEnabledAsync(bool enabled)
    {
        return ProfileAppService.SetTwoFactorEnabledAsync(enabled);
    }

    [HttpGet]
    [Route("can-enable-two-factor")]
    public Task<bool> CanEnableTwoFactorAsync()
    {
        return ProfileAppService.CanEnableTwoFactorAsync();
    }

    [HttpGet]
    [Route("timezones")]
    public virtual Task<List<NameValue>> GetTimezonesAsync()
    {
        return ProfileAppService.GetTimezonesAsync();
    }
}
