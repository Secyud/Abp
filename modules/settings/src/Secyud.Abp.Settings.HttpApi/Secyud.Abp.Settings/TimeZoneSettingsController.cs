using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Settings;

[RemoteService(Name = SettingsRemoteServiceConsts.RemoteServiceName)]
[Area(SettingsRemoteServiceConsts.ModuleName)]
[Route("api/settings/timezone")]
public class TimeZoneSettingsController(ITimeZoneSettingsAppService timeZoneSettingsAppService) : AbpControllerBase, ITimeZoneSettingsAppService
{
    [HttpGet]
    public Task<string?> GetAsync()
    {
        return timeZoneSettingsAppService.GetAsync();
    }

    [HttpGet]
    [Route("timezones")]
    public Task<List<NameValue>> GetTimezonesAsync()
    {
        return timeZoneSettingsAppService.GetTimezonesAsync();
    }

    [HttpPost]
    public Task UpdateAsync(string? timezone)
    {
        return timeZoneSettingsAppService.UpdateAsync(timezone);
    }
}
