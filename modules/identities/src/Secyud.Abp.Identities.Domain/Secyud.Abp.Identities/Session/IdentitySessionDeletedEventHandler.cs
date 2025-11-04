using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace Secyud.Abp.Identities.Session;

public class IdentitySessionDeletedEventHandler(IDistributedCache<IdentitySessionCacheItem> cache)
    : ILocalEventHandler<EntityDeletedEventData<IdentitySession>>, ITransientDependency
{
    protected IDistributedCache<IdentitySessionCacheItem> Cache { get; } = cache;

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEventData<IdentitySession> eventData)
    {
        await Cache.RemoveAsync(eventData.Entity.SessionId);
    }
}
