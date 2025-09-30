using Microsoft.AspNetCore.Components;
using Secyud.Abp.Settings.Localization;
using Volo.Abp;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Configuration;

namespace Secyud.Abp.Settings.Components.TimeZoneSettingGroup;

public partial class TimeZoneSettingGroupViewComponent
{
    [Inject]
    protected ITimeZoneSettingsAppService TimeZoneSettingsAppService { get; set; } = null!;

    [Inject]
    private ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; } = null!;

    [Inject]
    protected IUiMessageService UiMessageService { get; set; } = null!;

    protected UpdateTimezoneSettingsViewModel? TimezoneSettings { get; set; } = new();

    public TimeZoneSettingGroupViewComponent()
    {
        ObjectMapperContext = typeof(AbpSettingsBlazorModule);
        LocalizationResource = typeof(AbpSettingsResource);
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            TimezoneSettings = new UpdateTimezoneSettingsViewModel
            {
                Timezone = await TimeZoneSettingsAppService.GetAsync(),
                TimeZoneItems = await TimeZoneSettingsAppService.GetTimezonesAsync(),
            };
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OnSelectedValueChangedAsync(string? timezone)
    {
        TimezoneSettings!.Timezone = timezone;
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task UpdateSettingsAsync()
    {
        try
        {
            await TimeZoneSettingsAppService.UpdateAsync(TimezoneSettings!.Timezone);
            await CurrentApplicationConfigurationCacheResetService.ResetAsync();
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected class UpdateTimezoneSettingsViewModel
    {
        public string? Timezone { get; set; }
        public List<NameValue> TimeZoneItems { get; set; } = [];
    }
}