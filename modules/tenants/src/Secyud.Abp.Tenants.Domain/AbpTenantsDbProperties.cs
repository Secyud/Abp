using Volo.Abp.Data;

namespace Secyud.Abp.Tenants;

public static class AbpTenantsDbProperties
{
    public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

    public static string? DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpTenants";
}
