using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public class ConfigurationSettingsProvider(IConfiguration configuration) : ISettingsProvider, ITransientDependency
{
    public string Name => ConfigurationSettingValueProvider.ProviderName;

    protected IConfiguration Configuration { get; } = configuration;

    public virtual Task<string?> GetOrNullAsync(SettingDefinition setting, string? providerKey)
    {
        return Task.FromResult(Configuration[ConfigurationSettingValueProvider.ConfigurationNamePrefix + setting.Name]);
    }

    public virtual Task SetAsync(SettingDefinition setting, string value, string? providerKey)
    {
        throw new AbpException($"Can not set a setting value to the application configuration.");
    }

    public virtual Task ClearAsync(SettingDefinition setting, string? providerKey)
    {
        throw new AbpException($"Can not set a setting value to the application configuration.");
    }
}
