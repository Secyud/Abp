using Volo.Abp.Data;

namespace Secyud.Abp.Identities;

public static class AbpIdentitiesDbProperties
{
    public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

    public static string? DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpIdentity";
}
