using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Secyud.Abp.Settings;

public class UserDeletedEventHandler(ISettingManager settingManager) :
    IDistributedEventHandler<EntityDeletedEto<UserEto>>,
    ITransientDependency
{
    protected ISettingManager SettingManager { get; } = settingManager;

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEto<UserEto> eventData)
    {
        await SettingManager.DeleteAsync(UserSettingValueProvider.ProviderName, eventData.Entity.Id.ToString());
    }
}
