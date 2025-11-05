using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Secyud.Abp.Account.ExternalProviders;
using Secyud.Abp.Account.Localization;
using Secyud.Abp.Account.Settings;
using Secyud.Abp.Identities.Features;
using Secyud.Abp.Identities.Settings;
using Secyud.Abp.Settings;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace Secyud.Abp.Accounts;

[Authorize(AccountPermissions.SettingManagement)]
public class AccountSettingsAppService : ApplicationService, IAccountSettingsAppService
{
    protected ISettingManager SettingManager { get; }
    protected ExternalProviderSettingsHelper ExternalProviderSettingsHelper { get; }

    public AccountSettingsAppService(ISettingManager settingManager, ExternalProviderSettingsHelper externalProviderSettingsHelper)
    {
        SettingManager = settingManager;
        ExternalProviderSettingsHelper = externalProviderSettingsHelper;
        LocalizationResource = typeof(AbpAccountsResource);
    }

    public virtual async Task<AccountSettingsDto> GetAsync()
    {
        return new AccountSettingsDto
        {
            IsSelfRegistrationEnabled = await SettingProvider.GetAsync<bool>(AccountSettingNames.IsSelfRegistrationEnabled),
            EnableLocalLogin = await SettingProvider.GetAsync<bool>(AccountSettingNames.EnableLocalLogin),
            PreventEmailEnumeration = await SettingProvider.GetAsync<bool>(AccountSettingNames.PreventEmailEnumeration)
        };
    }

    public virtual async Task UpdateAsync(AccountSettingsDto input)
    {
        await SettingManager.SetForCurrentTenantAsync(AccountSettingNames.IsSelfRegistrationEnabled, input.IsSelfRegistrationEnabled.ToString());
        await SettingManager.SetForCurrentTenantAsync(AccountSettingNames.EnableLocalLogin, input.EnableLocalLogin.ToString());
        await SettingManager.SetForCurrentTenantAsync(AccountSettingNames.PreventEmailEnumeration, input.PreventEmailEnumeration.ToString());
    }

    public virtual async Task<AccountTwoFactorSettingsDto> GetTwoFactorAsync()
    {
        await CheckTwoFactorAvailableAsync();

        return new AccountTwoFactorSettingsDto
        {
            TwoFactorBehaviour = await IdentitiesTwoFactorBehaviourSettingHelper.Get(SettingProvider),
            IsRememberBrowserEnabled = await SettingProvider.GetAsync<bool>(AccountSettingNames.TwoFactorLogin.IsRememberBrowserEnabled),
            UsersCanChange = await SettingProvider.GetAsync<bool>(IdentitiesSettingNames.TwoFactor.UsersCanChange)
        };
    }

    public virtual async Task UpdateTwoFactorAsync(AccountTwoFactorSettingsDto input)
    {
        await CheckTwoFactorAvailableAsync();

            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.TwoFactor.Behaviour, input.TwoFactorBehaviour.ToString());
            if (input.TwoFactorBehaviour == IdentitiesTwoFactorBehaviour.Optional)
            {
                await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.TwoFactor.UsersCanChange, input.UsersCanChange.ToString());
            }

            await SettingManager.SetForCurrentTenantAsync(AccountSettingNames.TwoFactorLogin.IsRememberBrowserEnabled,
                input.IsRememberBrowserEnabled.ToString());
       
    }

    public virtual async Task<AccountExternalProviderSettingsDto> GetExternalProviderAsync()
    {
        return new AccountExternalProviderSettingsDto
        {
            VerifyPasswordDuringExternalLogin = await SettingProvider.GetAsync<bool>(AccountSettingNames.VerifyPasswordDuringExternalLogin),
            ExternalProviders = await ExternalProviderSettingsHelper.GetAllAsync()
        };
    }

    public virtual async Task UpdateExternalProviderAsync(AccountExternalProviderSettingsDto input)
    {
        await SettingManager.SetForCurrentTenantAsync(AccountSettingNames.VerifyPasswordDuringExternalLogin,
            input.VerifyPasswordDuringExternalLogin.ToString());
        foreach (var setting in input.ExternalProviders)
        {
            await ExternalProviderSettingsHelper.SetAsync(new ExternalProviderSettings
            {
                Name = setting.Name,
                Enabled = setting.Enabled,
                Properties = setting.Properties,
                SecretProperties = setting.SecretProperties
            });

            await CurrentUnitOfWork!.SaveChangesAsync();
        }
    }

    protected virtual async Task CheckTwoFactorAvailableAsync()
    {
        var behaviour = await IdentitiesTwoFactorBehaviourFeatureHelper.Get(FeatureChecker);
        if (behaviour == IdentitiesTwoFactorBehaviour.Disabled)
        {
            throw new BusinessException(message: L["TwoFactorHasBeenDisabled"]);
        }
    }

    public virtual async Task<AccountRecaptchaSettingsDto> GetRecaptchaAsync()
    {
        var settings = new AccountRecaptchaSettingsDto
        {
            UseCaptchaOnLogin = await SettingProvider.GetAsync<bool>(AccountSettingNames.Captcha.UseCaptchaOnLogin),
            UseCaptchaOnRegistration = await SettingProvider.GetAsync<bool>(AccountSettingNames.Captcha.UseCaptchaOnRegistration),
            VerifyBaseUrl = await SettingProvider.GetOrNullAsync(AccountSettingNames.Captcha.VerifyBaseUrl),
            SiteKey = await SettingProvider.GetOrNullAsync(AccountSettingNames.Captcha.SiteKey),
            SiteSecret = await SettingProvider.GetOrNullAsync(AccountSettingNames.Captcha.SiteSecret),
            Version = await SettingProvider.GetAsync<int>(AccountSettingNames.Captcha.Version),
            Score = await SettingProvider.GetAsync<double>(AccountSettingNames.Captcha.Score)
        };

        if (CurrentTenant.IsAvailable)
        {
            settings.SiteKey = await SettingManager.GetOrNullForTenantAsync(AccountSettingNames.Captcha.SiteKey, CurrentTenant.GetId(), false);
            settings.SiteSecret = await SettingManager.GetOrNullForTenantAsync(AccountSettingNames.Captcha.SiteSecret, CurrentTenant.GetId(), false);
        }

        return settings;
    }

    public virtual async Task UpdateRecaptchaAsync(AccountRecaptchaSettingsDto input)
    {
        if (!CurrentTenant.IsAvailable)
        {
            if ((input.UseCaptchaOnLogin || input.UseCaptchaOnRegistration) &&
                (input.SiteKey.IsNullOrWhiteSpace() || input.SiteSecret.IsNullOrWhiteSpace()))
            {
                throw new UserFriendlyException(L["InvalidSiteKeyOrSiteSecret"]);
            }

            await SettingManager.SetGlobalAsync(AccountSettingNames.Captcha.UseCaptchaOnLogin, input.UseCaptchaOnLogin.ToString());
            await SettingManager.SetGlobalAsync(AccountSettingNames.Captcha.UseCaptchaOnRegistration, input.UseCaptchaOnRegistration.ToString());
            await SettingManager.SetGlobalAsync(AccountSettingNames.Captcha.VerifyBaseUrl, input.VerifyBaseUrl);
            await SettingManager.SetGlobalAsync(AccountSettingNames.Captcha.SiteKey, input.SiteKey);
            await SettingManager.SetGlobalAsync(AccountSettingNames.Captcha.SiteSecret, input.SiteSecret);
            await SettingManager.SetGlobalAsync(AccountSettingNames.Captcha.Version, input.Version.ToString());
            await SettingManager.SetGlobalAsync(AccountSettingNames.Captcha.Score, input.Score.ToString(CultureInfo.InvariantCulture));
        }
        else
        {
            var globalVersion = (await SettingManager.GetOrNullGlobalAsync(AccountSettingNames.Captcha.Version))?.To<int>();

            if (globalVersion != input.Version &&
                (input.SiteKey.IsNullOrWhiteSpace() || input.SiteSecret.IsNullOrWhiteSpace()))
            {
                throw new UserFriendlyException(L["InvalidSiteKeyOrSiteSecret"]);
            }

            await SettingManager.SetForTenantAsync(CurrentTenant.GetId(), AccountSettingNames.Captcha.Version, input.Version.ToString());
            await SettingManager.SetForTenantAsync(CurrentTenant.GetId(), AccountSettingNames.Captcha.SiteKey,
                input.SiteKey.IsNullOrWhiteSpace() ? null : input.SiteKey);
            await SettingManager.SetForTenantAsync(CurrentTenant.GetId(), AccountSettingNames.Captcha.SiteSecret,
                input.SiteSecret.IsNullOrWhiteSpace() ? null : input.SiteSecret);
            await SettingManager.SetForTenantAsync(CurrentTenant.GetId(), AccountSettingNames.Captcha.Score, input.Score.ToString(CultureInfo.InvariantCulture));
        }
    }
}