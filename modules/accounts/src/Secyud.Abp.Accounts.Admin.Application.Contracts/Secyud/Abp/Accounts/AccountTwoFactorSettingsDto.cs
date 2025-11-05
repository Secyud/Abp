using Secyud.Abp.Identities.Features;

namespace Secyud.Abp.Accounts;

public class AccountTwoFactorSettingsDto
{
    public IdentitiesTwoFactorBehaviour TwoFactorBehaviour { get; set; }

    public bool IsRememberBrowserEnabled { get; set; }

    public bool UsersCanChange { get; set; }
}
