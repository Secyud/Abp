using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Features.EntityFrameworkCore;

public class EfCoreFeatureValueRepository : EfCoreRepository<IFeaturesDbContext, FeatureValue, Guid>, IFeatureValueRepository
{
    public EfCoreFeatureValueRepository(IDbContextProvider<IFeaturesDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<FeatureValue?> FindAsync(
        string name,
        string? providerName,
        string? providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(s => s.Name == name && s.ProviderName == providerName && s.ProviderKey == providerKey, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<FeatureValue>> FindAllAsync(
        string name,
        string? providerName,
        string? providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(
                s => s.Name == name && s.ProviderName == providerName && s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<FeatureValue>> GetListAsync(
        string? providerName,
        string? providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(
                s => s.ProviderName == providerName && s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task DeleteAsync(
        string? providerName,
        string? providerKey,
        CancellationToken cancellationToken = default)
    {
        await DeleteAsync(s => s.ProviderName == providerName && s.ProviderKey == providerKey, cancellationToken: GetCancellationToken(cancellationToken));
    }
}
