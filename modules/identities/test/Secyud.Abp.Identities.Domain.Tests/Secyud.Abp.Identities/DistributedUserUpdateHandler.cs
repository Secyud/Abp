using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Testing.Utils;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities;

public class DistributedUserUpdateHandler(ITestCounter testCounter) : IDistributedEventHandler<EntityUpdatedEto<UserEto>>, ITransientDependency
{
    public Task HandleEventAsync(EntityUpdatedEto<UserEto> eventData)
    {
        if (eventData.Entity.UserName == "john.nash")
        {
            testCounter.Increment("EntityUpdatedEto<UserEto>");
        }

        return Task.CompletedTask;
    }
}
