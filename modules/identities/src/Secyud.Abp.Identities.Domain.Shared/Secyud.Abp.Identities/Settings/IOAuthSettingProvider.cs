namespace Secyud.Abp.Identities.Settings;

public interface IOAuthSettingProvider
{
    Task<string?> GetClientIdAsync();

    Task<string?> GetClientSecretAsync();

    Task<string?> GetAuthorityAsync();

    Task<string?> GetScopeAsync();

    Task<bool> GetRequireHttpsMetadataAsync();
    
    Task<bool> GetValidateEndpointsAsync();

    Task<bool> GetValidateIssuerNameAsync();
}