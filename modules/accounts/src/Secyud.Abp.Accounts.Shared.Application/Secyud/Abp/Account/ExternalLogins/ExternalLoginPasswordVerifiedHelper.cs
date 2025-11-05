using Secyud.Abp.Account.Settings;
using Secyud.Abp.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Secyud.Abp.Account.ExternalLogins;

public class ExternalLoginPasswordVerifiedHelper(
    ISettingManager settingManager,
    IJsonSerializer jsonSerializer) : ITransientDependency
{
    public virtual async Task<bool> HasPasswordVerifiedAsync(Guid userId, string loginProvider, string providerKey)
    {
        var settingString = await settingManager.GetOrNullForUserAsync(
            AccountSettingNames.ExternalLoginPasswordVerified,
            userId,
            fallback: false);

        if (settingString.IsNullOrWhiteSpace())
        {
            return false;
        }

        var settings = jsonSerializer.Deserialize<List<ExternalLoginSetting>>(settingString);
        return settings.Any(s => s.LoginProvider == loginProvider && s.ProviderKey == providerKey);
    }

    public virtual async Task SetPasswordVerifiedAsync(Guid userId, string loginProvider, string providerKey)
    {
        var settingString = await settingManager.GetOrNullForUserAsync(
            AccountSettingNames.ExternalLoginPasswordVerified,
            userId,
            fallback: false);

        var settings = settingString.IsNullOrWhiteSpace()
            ? new List<ExternalLoginSetting>()
            : jsonSerializer.Deserialize<List<ExternalLoginSetting>>(settingString);

        settings.RemoveAll(s => s.LoginProvider == loginProvider && s.ProviderKey == providerKey);
        settings.Add(new ExternalLoginSetting(loginProvider, providerKey));

        await settingManager.SetForUserAsync(
            userId,
            AccountSettingNames.ExternalLoginPasswordVerified,
            jsonSerializer.Serialize(settings),
            forceToSet: true
        );
    }

    public virtual async Task RemovePasswordVerifiedAsync(Guid userId, string loginProvider, string providerKey)
    {
        var settingString = await settingManager.GetOrNullForUserAsync(
            AccountSettingNames.ExternalLoginPasswordVerified,
            userId,
            fallback: false);

        if (settingString.IsNullOrWhiteSpace())
        {
            return;
        }

        var settings = jsonSerializer.Deserialize<List<ExternalLoginSetting>>(settingString);
        settings.RemoveAll(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);

        await settingManager.SetForUserAsync(
            userId,
            AccountSettingNames.ExternalLoginPasswordVerified,
            jsonSerializer.Serialize(settings),
            forceToSet: true
        );
    }
}
