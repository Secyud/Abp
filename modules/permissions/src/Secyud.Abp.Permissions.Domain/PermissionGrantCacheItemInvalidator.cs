using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Permissions;

public class PermissionGrantCacheItemInvalidator(
    IDistributedCache<PermissionGrantCacheItem, PermissionGrantCacheKey> cache,
    ICurrentTenant currentTenant)
    : ILocalEventHandler<EntityChangedEventData<PermissionGrant>>,
        ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; } = currentTenant;

    protected IDistributedCache<PermissionGrantCacheItem, PermissionGrantCacheKey> Cache { get; } = cache;

    public virtual async Task HandleEventAsync(EntityChangedEventData<PermissionGrant> eventData)
    {
        var cacheKey = new  PermissionGrantCacheKey(
            eventData.Entity.Name,
            eventData.Entity.ProviderName,
            eventData.Entity.ProviderKey
        );

        using (CurrentTenant.Change(eventData.Entity.TenantId))
        {
            await Cache.RemoveAsync(cacheKey, considerUow: true);
        }
    }
}