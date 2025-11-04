namespace Secyud.Abp.Identities;

public interface IIdentityDataSeeder
{
    Task<IdentityDataSeedResult> SeedAsync(
        string adminEmail,
        string adminPassword,
        Guid? tenantId = null,
        string? adminUserName = null);
}
