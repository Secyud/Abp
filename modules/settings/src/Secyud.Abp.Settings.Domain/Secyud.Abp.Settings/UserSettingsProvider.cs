using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace Secyud.Abp.Settings;

public class UserSettingsProvider(ISettingsStore settingsStore, ICurrentUser currentUser)
    : SettingsProvider(settingsStore), ITransientDependency
{
    public override string Name => UserSettingValueProvider.ProviderName;

    protected ICurrentUser CurrentUser { get; } = currentUser;

    protected override string? NormalizeProviderKey(string? providerKey)
    {
        if (providerKey != null)
        {
            return providerKey;
        }

        return CurrentUser.Id?.ToString();
    }
}