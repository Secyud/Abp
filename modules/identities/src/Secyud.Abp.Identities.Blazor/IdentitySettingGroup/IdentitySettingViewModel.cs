namespace Secyud.Abp.Identities.IdentitySettingGroup;

public class IdentitySettingViewModel
{
    public IdentitySettingsDto IdentitySettings { get; set; } = null!;

    public IdentityLdapSettingsDto? IdentityLdapSettings { get; set; }

    public IdentityOAuthSettingsDto? IdentityOAuthSettings { get; set; }

    public IdentitySessionSettingsDto IdentitySessionSettings { get; set; } = null!;
}