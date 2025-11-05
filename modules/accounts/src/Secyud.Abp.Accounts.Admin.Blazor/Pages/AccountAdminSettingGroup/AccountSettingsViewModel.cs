using Secyud.Abp.Account;

namespace Secyud.Abp.Accounts.Pages.AccountAdminSettingGroup;

public class AccountSettingsViewModel
{

    public AccountTwoFactorSettingsDto? AccountTwoFactorSettings { get; set; }
    public required AccountSettingsDto AccountSettings { get; set; }

    public required AccountRecaptchaSettingsDto AccountRecaptchaSettings { get; set; }

    public required AccountExternalProviderSettingsDto AccountExternalProviderSettings { get; set; } 
}