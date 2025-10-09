using Volo.Abp.Data;

namespace Secyud.Abp.Features;

public static class AbpFeaturesDbProperties
{
    public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

    public static string? DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpFeatures";
}