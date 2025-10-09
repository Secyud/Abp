using Secyud.Abp.Settings.EntityFrameworkCore;

namespace Secyud.Abp.Settings;

public class SettingsDomainTestBase : SettingsTestBase<AbpSettingsDomainTestModule>
{
    protected virtual void UsingDbContext(Action<ISettingsDbContext> action)
    {
        using (var dbContext = GetRequiredService<ISettingsDbContext>())
        {
            action.Invoke(dbContext);
        }
    }

    protected virtual T UsingDbContext<T>(Func<ISettingsDbContext, T> action)
    {
        using (var dbContext = GetRequiredService<ISettingsDbContext>())
        {
            return action.Invoke(dbContext);
        }
    }

    protected List<Setting> GetSettingsFromDbContext(string entityType, string entityId, string name)
    {
        return UsingDbContext(context =>
            context.Settings.Where(
                s =>
                    s.ProviderName == entityType &&
                    s.ProviderKey == entityId &&
                    s.Name == name
            ).ToList()
        );
    }
}
