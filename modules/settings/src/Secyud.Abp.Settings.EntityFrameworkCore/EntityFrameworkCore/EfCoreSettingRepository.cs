using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Settings.EntityFrameworkCore;

public class EfCoreSettingRepository(IDbContextProvider<ISettingsDbContext> dbContextProvider)
    : EfCoreRepository<ISettingsDbContext, Setting, Guid>(dbContextProvider), ISettingRepository
{
    public virtual async Task<Setting?> FindAsync(
        string name,
        string? providerName,
        string? providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(
                s => s.Name == name && s.ProviderName == providerName && s.ProviderKey == providerKey,
                GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Setting>> GetListAsync(
        string? providerName,
        string? providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(s => s.ProviderName == providerName && s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Setting>> GetListAsync(
        string[] names,
        string? providerName,
        string? providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(s => names.Contains(s.Name) && s.ProviderName == providerName && s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }
}