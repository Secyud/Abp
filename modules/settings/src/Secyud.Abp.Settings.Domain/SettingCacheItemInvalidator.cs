using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Secyud.Abp.Settings;

public class SettingCacheItemInvalidator(IDistributedCache<SettingCacheItem, SettingCacheKey> cache) :
    ILocalEventHandler<EntityChangedEventData<Setting>>, ITransientDependency
{
    protected IDistributedCache<SettingCacheItem, SettingCacheKey> Cache { get; } = cache;

    public virtual async Task HandleEventAsync(EntityChangedEventData<Setting> eventData)
    {
        var cacheKey = new SettingCacheKey(
            eventData.Entity.Name,
            eventData.Entity.ProviderName,
            eventData.Entity.ProviderKey
        );

        await Cache.RemoveAsync(cacheKey, considerUow: true);
    }
}