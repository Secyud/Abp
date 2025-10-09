using Volo.Abp.Data;

namespace Secyud.Abp.Settings;

public static class AbpSettingsDbProperties
{
    public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

    public static string? DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpSettings";
}
