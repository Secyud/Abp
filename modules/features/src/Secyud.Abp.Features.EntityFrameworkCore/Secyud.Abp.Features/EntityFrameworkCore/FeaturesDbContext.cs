using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Features.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpFeaturesDbProperties.ConnectionStringName)]
public class FeaturesDbContext(DbContextOptions<FeaturesDbContext> options) : AbpDbContext<FeaturesDbContext>(options), IFeaturesDbContext
{
    public DbSet<FeatureGroupDefinitionRecord> FeatureGroups { get; set; }

    public DbSet<FeatureDefinitionRecord> Features { get; set; }

    public DbSet<FeatureValue> FeatureValues { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureFeatures();
    }
}