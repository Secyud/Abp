using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public class GlobalSettingsProvider(ISettingsStore settingsStore) : SettingsProvider(settingsStore), ITransientDependency
{
    public override string Name => GlobalSettingValueProvider.ProviderName;

    protected override string? NormalizeProviderKey(string? providerKey)
    {
        return null;
    }
}