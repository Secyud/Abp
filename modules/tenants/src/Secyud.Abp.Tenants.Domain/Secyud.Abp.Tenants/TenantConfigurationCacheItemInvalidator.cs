using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Local;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Tenants;

[LocalEventHandlerOrder(-1)]
public class TenantConfigurationCacheItemInvalidator(IDistributedCache<TenantConfigurationCacheItem> cache) :
    ILocalEventHandler<EntityChangedEventData<Tenant>>,
    ILocalEventHandler<TenantChangedEvent>,
    ITransientDependency
{
    protected IDistributedCache<TenantConfigurationCacheItem> Cache { get; } = cache;

    public virtual async Task HandleEventAsync(EntityChangedEventData<Tenant> eventData)
    {
        if (eventData is EntityCreatedEventData<Tenant>)
        {
            return;
        }

        await ClearCacheAsync(eventData.Entity.Id, eventData.Entity.NormalizedName);
    }

    public virtual async Task HandleEventAsync(TenantChangedEvent eventData)
    {
        await ClearCacheAsync(eventData.Id, eventData.NormalizedName);
    }

    protected virtual async Task ClearCacheAsync(Guid? id, string? normalizedName)
    {
        await Cache.RemoveManyAsync(
        [
            TenantConfigurationCacheItem.CalculateCacheKey(id, null),
                TenantConfigurationCacheItem.CalculateCacheKey(null, normalizedName),
                TenantConfigurationCacheItem.CalculateCacheKey(id, normalizedName)
        ], considerUow: true);
    }
}