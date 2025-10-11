﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;
using Volo.Abp.Uow;

namespace Secyud.Abp.Settings;

public class SettingManager(
    IOptions<SettingsOptions> options,
    ISettingDefinitionManager settingDefinitionManager,
    ISettingEncryptionService settingEncryptionService,
    ISettingsStore settingsStore,
    IServiceProvider serviceProvider)
    : ISettingManager, ISingletonDependency
{
    private readonly Lazy<List<ISettingsProvider>> _settingsProviders = new(() =>
    {
        return options.Value
            .Providers
            .Select(c => (ISettingsProvider)
                serviceProvider.GetRequiredService(c))
            .ToList();
    });

    protected ISettingDefinitionManager SettingDefinitionManager { get; } = settingDefinitionManager;
    protected ISettingEncryptionService SettingEncryptionService { get; } = settingEncryptionService;
    protected ISettingsStore SettingsStore { get; } = settingsStore;
    protected List<ISettingsProvider> SettingsProviders => _settingsProviders.Value;

    public virtual Task<string?> GetOrNullAsync(string name, string? providerName, string? providerKey, bool fallback = true)
    {
        Check.NotNull(name, nameof(name));
        Check.NotNull(providerName, nameof(providerName));

        return GetOrNullInternalAsync(name, providerName, providerKey, fallback);
    }

    public virtual async Task<List<SettingValue>> GetAllAsync(string? providerName, string? providerKey, bool fallback = true)
    {
        Check.NotNull(providerName, nameof(providerName));

        var settingDefinitions = await SettingDefinitionManager.GetAllAsync();
        var providers = Enumerable.Reverse(SettingsProviders)
            .SkipWhile(c => c.Name != providerName);

        if (!fallback)
        {
            providers = providers.TakeWhile(c => c.Name == providerName);
        }

        var providerList = providers.Reverse().ToList();

        if (!providerList.Any())
        {
            return [];
        }

        var settingValues = new Dictionary<string, SettingValue>();

        foreach (var setting in settingDefinitions)
        {
            string? value = null;

            if (setting.IsInherited)
            {
                foreach (var provider in providerList)
                {
                    var providerValue = await provider.GetOrNullAsync(
                        setting,
                        provider.Name == providerName ? providerKey : null
                    );
                    if (providerValue != null)
                    {
                        value = providerValue;
                    }
                }
            }
            else
            {
                value = await providerList[0].GetOrNullAsync(
                    setting,
                    providerKey
                );
            }

            if (setting.IsEncrypted)
            {
                value = SettingEncryptionService.Decrypt(setting, value);
            }

            if (value != null)
            {
                settingValues[setting.Name] = new SettingValue(setting.Name, value);
            }
        }

        return settingValues.Values.ToList();
    }

    public virtual async Task SetAsync(string name, string? value, string? providerName, string? providerKey, bool forceToSet = false)
    {
        Check.NotNull(name, nameof(name));
        Check.NotNull(providerName, nameof(providerName));

        var setting = await SettingDefinitionManager.GetAsync(name);

        var providers = Enumerable
            .Reverse(SettingsProviders)
            .SkipWhile(p => p.Name != providerName)
            .ToList();

        if (!providers.Any())
        {
            throw new AbpException($"Unknown setting value provider: {providerName}");
        }

        if (setting.IsEncrypted)
        {
            value = SettingEncryptionService.Encrypt(setting, value);
        }

        if (providers.Count > 1 && !forceToSet && setting.IsInherited && value != null)
        {
            var fallbackValue = await GetOrNullInternalAsync(name, providers[1].Name, null);
            if (fallbackValue == value)
            {
                //Clear the value if it's same as it's fallback value
                value = null;
            }
        }

        providers = providers
            .TakeWhile(p => p.Name == providerName)
            .ToList(); //Getting list for case of there are more than one provider with same providerName

        if (value == null)
        {
            foreach (var provider in providers)
            {
                await provider.ClearAsync(setting, providerKey);
            }
        }
        else
        {
            foreach (var provider in providers)
            {
                await provider.SetAsync(setting, value, providerKey);
            }
        }
    }

    public virtual async Task DeleteAsync(string? providerName, string? providerKey)
    {
        var settings = await SettingsStore.GetListAsync(providerName, providerKey);
        foreach (var setting in settings)
        {
            await SettingsStore.DeleteAsync(setting.Name, providerName, providerKey);
        }
    }

    protected virtual async Task<string?> GetOrNullInternalAsync(string name, string? providerName, string? providerKey, bool fallback = true)
    {
        var setting = await SettingDefinitionManager.GetAsync(name);
        var providers = Enumerable
            .Reverse(SettingsProviders);

        if (providerName != null)
        {
            providers = providers.SkipWhile(c => c.Name != providerName);
        }

        if (!fallback || !setting.IsInherited)
        {
            providers = providers.TakeWhile(c => c.Name == providerName);
        }

        string? value = null;
        foreach (var provider in providers)
        {
            value = await provider.GetOrNullAsync(
                setting,
                provider.Name == providerName ? providerKey : null
            );

            if (value != null)
            {
                break;
            }
        }

        if (setting.IsEncrypted)
        {
            value = SettingEncryptionService.Decrypt(setting, value);
        }

        return value;
    }
}