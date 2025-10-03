using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Features.EntityFrameworkCore;

public class EfCoreFeatureGroupDefinitionRecordRepository :
    EfCoreRepository<IFeaturesDbContext, FeatureGroupDefinitionRecord, Guid>,
    IFeatureGroupDefinitionRecordRepository
{
    public EfCoreFeatureGroupDefinitionRecordRepository(
        IDbContextProvider<IFeaturesDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
