using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace Secyud.Abp.Identities.Settings;

public class OAuthSettingProvider(ISettingProvider settingProvider) : IOAuthSettingProvider, ITransientDependency
{
    protected ISettingProvider SettingProvider { get; } = settingProvider;

    public async Task<string?> GetClientIdAsync()
    {
        return await SettingProvider.GetOrNullAsync(IdentitiesSettingNames.OAuthLogin.ClientId);
    }

    public async Task<string?> GetClientSecretAsync()
    {
        return await SettingProvider.GetOrNullAsync(IdentitiesSettingNames.OAuthLogin.ClientSecret);
    }

    public async Task<string?> GetAuthorityAsync()
    {
        return await SettingProvider.GetOrNullAsync(IdentitiesSettingNames.OAuthLogin.Authority);
    }

    public async Task<string?> GetScopeAsync()
    {
        return await SettingProvider.GetOrNullAsync(IdentitiesSettingNames.OAuthLogin.Scope);
    }

    public async Task<bool> GetRequireHttpsMetadataAsync()
    {
        return (await SettingProvider.GetOrNullAsync(IdentitiesSettingNames.OAuthLogin.RequireHttpsMetadata))?.To<bool>() ?? true;
    }

    public async Task<bool> GetValidateEndpointsAsync()
    {
        return (await SettingProvider.GetOrNullAsync(IdentitiesSettingNames.OAuthLogin.ValidateEndpoints))?.To<bool>() ?? true;
    }

    public async Task<bool> GetValidateIssuerNameAsync()
    {
        return (await SettingProvider.GetOrNullAsync(IdentitiesSettingNames.OAuthLogin.ValidateIssuerName))?.To<bool>() ?? true;
    }
}