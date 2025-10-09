using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public class SettingStore(ISettingsStore store) : ISettingStore, ITransientDependency
{
    protected ISettingsStore Store { get; } = store;

    public virtual Task<string?> GetOrNullAsync(string name, string? providerName, string? providerKey)
    {
        return Store.GetOrNullAsync(name, providerName, providerKey);
    }

    public virtual Task<List<SettingValue>> GetAllAsync(string[] names, string? providerName, string? providerKey)
    {
        return Store.GetListAsync(names, providerName, providerKey);
    }
}
