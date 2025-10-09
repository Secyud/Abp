using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Settings.EntityFrameworkCore;

public class EfCoreSettingDefinitionRecordRepository(IDbContextProvider<ISettingsDbContext> dbContextProvider)
    : EfCoreRepository<ISettingsDbContext, SettingDefinitionRecord, Guid>(dbContextProvider), ISettingDefinitionRecordRepository
{
    public virtual async Task<SettingDefinitionRecord?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }
}