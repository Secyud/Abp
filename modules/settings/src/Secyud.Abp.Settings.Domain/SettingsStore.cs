using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Settings;
using Volo.Abp.Uow;

namespace Secyud.Abp.Settings;

public class SettingsStore(
    ISettingRepository settingRepository,
    IGuidGenerator guidGenerator,
    IDistributedCache<SettingCacheItem, SettingCacheKey> cache,
    ISettingDefinitionManager settingDefinitionManager)
    : ISettingsStore, ITransientDependency
{
    protected IDistributedCache<SettingCacheItem, SettingCacheKey> Cache { get; } = cache;
    protected ISettingDefinitionManager SettingDefinitionManager { get; } = settingDefinitionManager;
    protected ISettingRepository SettingRepository { get; } = settingRepository;
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;

    [UnitOfWork]
    public virtual async Task<string?> GetOrNullAsync(string name, string? providerName, string? providerKey)
    {
        return (await GetCacheItemsAsync([name], providerName, providerKey)).FirstOrDefault().Value?.Value;
    }

    [UnitOfWork]
    public virtual async Task SetAsync(string name, string value, string? providerName, string? providerKey)
    {
        var setting = await SettingRepository.FindAsync(name, providerName, providerKey);
        if (setting is null)
        {
            setting = new Setting(GuidGenerator.Create(), name, value, providerName, providerKey);
            await SettingRepository.InsertAsync(setting);
        }
        else
        {
            setting.Value = value;
            await SettingRepository.UpdateAsync(setting);
        }

        await Cache.SetAsync(
            new SettingCacheKey(name, providerName, providerKey),
            new SettingCacheItem(setting.Value), considerUow: true);
    }

    public virtual async Task<List<SettingValue>> GetListAsync(string? providerName, string? providerKey)
    {
        var settings = await SettingRepository.GetListAsync(providerName, providerKey);
        return settings.Select(s => new SettingValue(s.Name, s.Value)).ToList();
    }

    [UnitOfWork]
    public virtual async Task DeleteAsync(string name, string? providerName, string? providerKey)
    {
        var setting = await SettingRepository.FindAsync(name, providerName, providerKey);
        if (setting is not null)
        {
            await SettingRepository.DeleteAsync(setting);
            await Cache.RemoveAsync(new SettingCacheKey(name, providerName, providerKey), considerUow: true);
        }
    }

    [UnitOfWork]
    public async Task<List<SettingValue>> GetListAsync(string[] names, string? providerName, string? providerKey)
    {
        Check.NotNullOrEmpty(names, nameof(names));

        var result = new List<SettingValue>();

        var cacheItems = await GetCacheItemsAsync(names, providerName, providerKey);
        foreach (var item in cacheItems)
        {
            result.Add(new SettingValue(item.Key.Name, item.Value?.Value));
        }

        return result;
    }

    protected virtual async Task<KeyValuePair<SettingCacheKey, SettingCacheItem?>[]> GetCacheItemsAsync(
        string[] names, string? providerName, string? providerKey)
    {
        var cacheKeys = names.Select(x => new SettingCacheKey(x, providerName, providerKey)).ToList();

        var cacheItems = await Cache.GetManyAsync(cacheKeys, considerUow: true);

        if (cacheItems.All(x => x.Value != null))
        {
            return cacheItems;
        }

        var notCacheKeys = cacheItems.Where(x => x.Value == null)
            .Select(x => x.Key).ToList();

        var newCacheItems = await SetCacheItemsAsync(providerName, providerKey, notCacheKeys);

        var result = new KeyValuePair<SettingCacheKey, SettingCacheItem?>[names.Length];
        for (var i = 0; i < cacheItems.Length; i++)
        {
            var (key, item) = cacheItems[i];
            item ??= newCacheItems.FirstOrDefault(x => x.Key == key).Value;
            result[i] = new KeyValuePair<SettingCacheKey, SettingCacheItem?>(key, item);
        }

        return result;
    }

    private async Task<List<KeyValuePair<SettingCacheKey, SettingCacheItem>>> SetCacheItemsAsync(
        string? providerName, string? providerKey, List<SettingCacheKey> notCacheKeys)
    {
        var settingDefinitions = (await SettingDefinitionManager.GetAllAsync())
            .Where(x => notCacheKeys.Any(k => k.Name == x.Name));

        var settingsDictionary =
            (await SettingRepository.GetListAsync(notCacheKeys.Select(u => u.Name).ToArray(), providerName, providerKey))
            .ToDictionary(s => s.Name, s => s.Value);

        var cacheItems = new List<KeyValuePair<SettingCacheKey, SettingCacheItem>>();

        foreach (var settingDefinition in settingDefinitions)
        {
            var settingValue = settingsDictionary.GetOrDefault(settingDefinition.Name);
            cacheItems.Add(
                new KeyValuePair<SettingCacheKey, SettingCacheItem>(
                    new SettingCacheKey(settingDefinition.Name, providerName, providerKey),
                    new SettingCacheItem(settingValue)
                )
            );
        }

        await Cache.SetManyAsync(cacheItems, considerUow: true);

        return cacheItems;
    }
}