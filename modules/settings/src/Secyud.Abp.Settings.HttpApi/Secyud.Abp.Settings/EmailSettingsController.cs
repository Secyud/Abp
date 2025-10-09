using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Settings;

[RemoteService(Name = SettingsRemoteServiceConsts.RemoteServiceName)]
[Area(SettingsRemoteServiceConsts.ModuleName)]
[Route("api/settings/emailing")]
public class EmailSettingsController(IEmailSettingsAppService emailSettingsAppService) : AbpControllerBase, IEmailSettingsAppService
{
    [HttpGet]
    public Task<EmailSettingsDto> GetAsync()
    {
        return emailSettingsAppService.GetAsync();
    }

    [HttpPost]
    public Task UpdateAsync(UpdateEmailSettingsDto input)
    {
        return emailSettingsAppService.UpdateAsync(input);
    }

    [HttpPost("send-test-email")]
    public Task SendTestEmailAsync(SendTestEmailInput input)
    {
        return emailSettingsAppService.SendTestEmailAsync(input);
    }
}
