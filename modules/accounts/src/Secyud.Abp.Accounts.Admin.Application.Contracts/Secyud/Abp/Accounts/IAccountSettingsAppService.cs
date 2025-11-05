using Volo.Abp.Application.Services;

namespace Secyud.Abp.Accounts;

public interface IAccountSettingsAppService : IApplicationService
{
    Task<AccountSettingsDto> GetAsync();

    Task UpdateAsync(AccountSettingsDto input);

    Task<AccountTwoFactorSettingsDto> GetTwoFactorAsync();

    Task UpdateTwoFactorAsync(AccountTwoFactorSettingsDto input);

    Task<AccountRecaptchaSettingsDto> GetRecaptchaAsync();

    Task UpdateRecaptchaAsync(AccountRecaptchaSettingsDto input);

    Task<AccountExternalProviderSettingsDto> GetExternalProviderAsync();

    Task UpdateExternalProviderAsync(AccountExternalProviderSettingsDto input);
}
