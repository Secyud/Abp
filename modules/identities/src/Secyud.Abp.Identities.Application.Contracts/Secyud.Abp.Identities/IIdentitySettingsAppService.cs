using Volo.Abp.Application.Services;

namespace Secyud.Abp.Identities;

public interface IIdentitySettingsAppService : IApplicationService
{
    Task<IdentitySettingsDto> GetAsync();

    Task UpdateAsync(IdentitySettingsDto input);

    Task<IdentityLdapSettingsDto> GetLdapAsync();

    Task UpdateLdapAsync(IdentityLdapSettingsDto? input);

    Task<IdentityOAuthSettingsDto> GetOAuthAsync();

    Task UpdateOAuthAsync(IdentityOAuthSettingsDto? input);

    Task<IdentitySessionSettingsDto> GetSessionAsync();

    Task UpdateSessionAsync(IdentitySessionSettingsDto? input);
}
