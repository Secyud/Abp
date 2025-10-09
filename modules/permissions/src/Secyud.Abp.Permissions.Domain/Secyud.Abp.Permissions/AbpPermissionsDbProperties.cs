using Volo.Abp.Data;

namespace Secyud.Abp.Permissions;

public static class AbpPermissionsDbProperties
{
    public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

    public static string? DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpPermissions";
}
