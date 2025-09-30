using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public class TenantSettingsProvider(ISettingsStore settingsStore, ICurrentTenant currentTenant)
    : SettingsProvider(settingsStore), ITransientDependency
{
    public override string Name => TenantSettingValueProvider.ProviderName;

    protected ICurrentTenant CurrentTenant { get; } = currentTenant;

    protected override string? NormalizeProviderKey(string? providerKey)
    {
        if (providerKey != null)
        {
            return providerKey;
        }

        return CurrentTenant.Id?.ToString();
    }
}