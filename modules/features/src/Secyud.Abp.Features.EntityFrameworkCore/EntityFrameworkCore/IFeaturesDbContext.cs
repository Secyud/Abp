using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Features.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpFeaturesDbProperties.ConnectionStringName)]
public interface IFeaturesDbContext : IEfCoreDbContext
{
    DbSet<FeatureGroupDefinitionRecord> FeatureGroups { get; }

    DbSet<FeatureDefinitionRecord> Features { get; }

    DbSet<FeatureValue> FeatureValues { get; }
}
