namespace Secyud.Abp.Permissions;

public interface IPermissionDataSeeder
{
    Task SeedAsync(
        string providerName,
        string providerKey,
        IEnumerable<string> grantedPermissions,
        Guid? tenantId = null);
}