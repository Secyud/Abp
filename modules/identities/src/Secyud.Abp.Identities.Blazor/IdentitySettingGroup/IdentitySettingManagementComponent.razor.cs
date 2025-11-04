using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Secyud.Abp.Identities.Features;
using Secyud.Abp.Identities.Localization;
using Secyud.Secits.Blazor.Validations;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.Features;
using Volo.Abp.Ldap.Localization;

namespace Secyud.Abp.Identities.IdentitySettingGroup;

public partial class IdentitySettingManagementComponent : AbpComponentBase
{
    public IdentitySettingManagementComponent()
    {
        LocalizationResource = typeof(AbpIdentitiesResource);
    }

    [Inject]
    protected IIdentitySettingsAppService IdentitySettingsAppService { get; set; } = null!;

    [Inject]
    private ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; } = null!;

    [Inject]
    protected IUiMessageService UiMessageService { get; set; } = null!;

    [Inject]
    protected IStringLocalizer<LdapResource> LdapLocalizer { get; set; } = null!;

    [Inject]
    protected IFeatureChecker FeatureChecker { get; set; } = null!;

    protected IdentitySettingViewModel? Settings;

    protected ValidationForm? IdentitySettingsForm;
    protected ValidationForm? IdentityLdapSettingsForm;
    protected ValidationForm? IdentityOAuthSettingsForm;
    protected ValidationForm? IdentitySessionSettingsForm;

    protected override async Task OnInitializedAsync()
    {
        Settings = new IdentitySettingViewModel
        {
            IdentitySettings = await IdentitySettingsAppService.GetAsync()
        };

        if (await FeatureChecker.IsEnabledAsync(IdentitiesFeature.EnableLdapLogin))
        {
            Settings.IdentityLdapSettings = await IdentitySettingsAppService.GetLdapAsync();
        }

        if (await FeatureChecker.IsEnabledAsync(IdentitiesFeature.EnableOAuthLogin))
        {
            Settings.IdentityOAuthSettings = await IdentitySettingsAppService.GetOAuthAsync();
        }

        Settings.IdentitySessionSettings = await IdentitySettingsAppService.GetSessionAsync();
    }

    protected virtual async Task UpdateSettingsAsync()
    {
        await ValidateAndSubmitAsync(IdentitySettingsForm, () =>
            IdentitySettingsAppService.UpdateAsync(Settings!.IdentitySettings));
    }

    protected virtual async Task UpdateOAuthSettingsAsync()
    {
        await ValidateAndSubmitAsync(IdentityOAuthSettingsForm, () =>
            IdentitySettingsAppService.UpdateOAuthAsync(Settings!.IdentityOAuthSettings));
    }

    protected virtual async Task UpdateLdapSettingsAsync()
    {
        await ValidateAndSubmitAsync(IdentityLdapSettingsForm, () =>
            IdentitySettingsAppService.UpdateLdapAsync(Settings!.IdentityLdapSettings));
    }

    protected virtual async Task UpdateSessionsSettingsAsync()
    {
        await ValidateAndSubmitAsync(IdentitySessionSettingsForm, () =>
            IdentitySettingsAppService.UpdateSessionAsync(Settings!.IdentitySessionSettings));
    }

    protected async Task ValidateAndSubmitAsync(ValidationForm? form, Func<Task> action)
    {
        try
        {
            if (form?.Validate() is false)
            {
                await Notify.Error(string.Join("\r\n", form.Fields.SelectMany(u => u.ValidationResults)));
                return;
            }

            await action();
            await CurrentApplicationConfigurationCacheResetService.ResetAsync();
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
}