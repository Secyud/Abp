using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Features.EntityFrameworkCore;

public class EfCoreFeatureDefinitionRecordRepository(IDbContextProvider<IFeaturesDbContext> dbContextProvider) :
    EfCoreRepository<IFeaturesDbContext, FeatureDefinitionRecord, Guid>(dbContextProvider),
    IFeatureDefinitionRecordRepository
{
    public virtual async Task<FeatureDefinitionRecord?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }
}
