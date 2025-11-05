using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Secyud.Abp.Account;
using Secyud.Abp.Account.ExternalProviders;
using Secyud.Abp.Account.Localization;
using Secyud.Abp.Identities.Features;
using Secyud.Abp.Identities.Localization;
using Secyud.Secits.Blazor;
using Secyud.Secits.Blazor.Validations;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Accounts.Pages.AccountAdminSettingGroup;

public partial class AccountAdminSettingManagementComponent
{
    [Inject]
    protected IAccountSettingsAppService AccountSettingsAppService { get; set; } = null!;

    [Inject]
    private ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; } = null!;

    [Inject]
    protected IUiMessageService UiMessageService { get; set; } = null!;

    [Inject]
    protected IStringLocalizer<AbpIdentitiesResource> IdentityLocalizer { get; set; } = null!;

    [Inject]
    protected IFeatureChecker FeatureChecker { get; set; } = null!;

    protected AccountSettingsViewModel Settings = null!;

    protected string SelectedTab = "AccountSettingsGeneral";

    protected Dictionary<string, bool> ExternalProviderUseHostSettings = null!;

    protected SFormLayout AccountCaptchaSettingsValidations = null!;

    public AccountAdminSettingManagementComponent()
    {
        LocalizationResource = typeof(AbpAccountsResource);
    }

    protected override async Task OnInitializedAsync()
    {
        Settings = new AccountSettingsViewModel
        {
            AccountSettings = await AccountSettingsAppService.GetAsync(),
            AccountRecaptchaSettings = await AccountSettingsAppService.GetRecaptchaAsync(),
            AccountExternalProviderSettings = await AccountSettingsAppService.GetExternalProviderAsync()
        };

        if (CurrentTenant.IsAvailable)
        {
            ExternalProviderUseHostSettings =
                Settings.AccountExternalProviderSettings.ExternalProviders.ToDictionary(x => x.Name, x => !x.IsValid());
        }

        if (await IdentitiesTwoFactorBehaviourFeatureHelper.Get(FeatureChecker) == IdentitiesTwoFactorBehaviour.Optional)
        {
            Settings.AccountTwoFactorSettings = await AccountSettingsAppService.GetTwoFactorAsync();
        }
    }

    public bool ShouldShowCaptchaSettings()
    {
        return
            !CurrentTenant.IsAvailable ||
            Settings.AccountRecaptchaSettings.UseCaptchaOnLogin ||
            Settings.AccountRecaptchaSettings.UseCaptchaOnRegistration;
    }

    public bool ShouldShowExternalProviderSettings()
    {
        return (!CurrentTenant.IsAvailable && Settings.AccountExternalProviderSettings.ExternalProviders.Count != 0) ||
               (CurrentTenant.IsAvailable && Settings.AccountExternalProviderSettings.ExternalProviders.Any(x => x.Enabled));
    }

    protected virtual async Task UpdateAccountSettings()
    {
        try
        {
            await AccountSettingsAppService.UpdateAsync(Settings.AccountSettings);

            await CurrentApplicationConfigurationCacheResetService.ResetAsync();

            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task UpdateCaptchaSettings()
    {
        try
        {
            await AccountSettingsAppService.UpdateRecaptchaAsync(Settings.AccountRecaptchaSettings);

            await CurrentApplicationConfigurationCacheResetService.ResetAsync();

            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task UpdateTwoFactorSettings()
    {
        try
        {
            if (Settings.AccountTwoFactorSettings is null) return;
            await AccountSettingsAppService.UpdateTwoFactorAsync(Settings.AccountTwoFactorSettings);

            await CurrentApplicationConfigurationCacheResetService.ResetAsync();
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual void OnExternalProviderUseHostSettingsChanged(ExternalProviderSettings providerSetting, bool value)
    {
        // use host settings
        if (value)
        {
            foreach (var property in providerSetting.Properties)
            {
                property.Value = null;
            }

            foreach (var property in providerSetting.SecretProperties)
            {
                property.Value = null;
            }
        }

        ExternalProviderUseHostSettings[providerSetting.Name] = value;
    }

    protected virtual async Task UpdateExternalProviderSettings()
    {
        try
        {
            var updateDto = new AccountExternalProviderSettingsDto
            {
                VerifyPasswordDuringExternalLogin = Settings.AccountExternalProviderSettings.VerifyPasswordDuringExternalLogin,
                ExternalProviders = Settings.AccountExternalProviderSettings.ExternalProviders.Select(x => new ExternalProviderSettings
                {
                    Enabled = x.Enabled,
                    Name = x.Name,
                    Properties = x.Properties,
                    SecretProperties = x.SecretProperties
                }).ToList()
            };

            await AccountSettingsAppService.UpdateExternalProviderAsync(updateDto);

            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
}