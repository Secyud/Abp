using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Secyud.Abp.Features;

public class FeatureValueCacheItemInvalidator(IDistributedCache<FeatureValueCacheItem, FeatureValueCacheKey> cache) :
    ILocalEventHandler<EntityChangedEventData<FeatureValue>>,
    ITransientDependency
{
    protected IDistributedCache<FeatureValueCacheItem, FeatureValueCacheKey> Cache { get; } = cache;

    public virtual async Task HandleEventAsync(EntityChangedEventData<FeatureValue> eventData)
    {
        var cacheKey = new FeatureValueCacheKey(
            eventData.Entity.Name,
            eventData.Entity.ProviderName,
            eventData.Entity.ProviderKey);

        await Cache.RemoveAsync(cacheKey, considerUow: true);
    }
}