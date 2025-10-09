using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Features.EntityFrameworkCore;

public class EfCoreFeatureGroupDefinitionRecordRepository(IDbContextProvider<IFeaturesDbContext> dbContextProvider) :
    EfCoreRepository<IFeaturesDbContext, FeatureGroupDefinitionRecord, Guid>(dbContextProvider),
    IFeatureGroupDefinitionRecordRepository;
