using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace Secyud.Abp.Settings;

[Authorize(SettingsPermissions.TimeZone)]
public class TimeZoneSettingsAppService(ISettingManager settingManager, ITimezoneProvider timezoneProvider)
    : SettingsAppServiceBase, ITimeZoneSettingsAppService
{
    protected ISettingManager SettingManager { get; } = settingManager;
    protected ITimezoneProvider TimezoneProvider { get; } = timezoneProvider;

    public virtual async Task<string?> GetAsync()
    {
        if (CurrentTenant.GetMultiTenancySide() == MultiTenancySides.Host)
        {
            return await SettingManager.GetOrNullGlobalAsync(TimingSettingNames.TimeZone);
        }

        return await SettingManager.GetOrNullForCurrentTenantAsync(TimingSettingNames.TimeZone);
    }

    public virtual Task<List<NameValue>> GetTimezonesAsync()
    {
        return Task.FromResult(TimeZoneHelper.GetTimezones(TimezoneProvider.GetWindowsTimezones()));
    }

    public virtual async Task UpdateAsync(string? timezone)
    {
        if (CurrentTenant.GetMultiTenancySide() == MultiTenancySides.Host)
        {
            await SettingManager.SetGlobalAsync(TimingSettingNames.TimeZone, timezone);
        }
        else
        {
            await SettingManager.SetForCurrentTenantAsync(TimingSettingNames.TimeZone, timezone);
        }
    }
}
