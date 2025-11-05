using Secyud.Abp.Account.ExternalProviders;

namespace Secyud.Abp.Accounts;

public class AccountExternalProviderSettingsDto
{
    public bool VerifyPasswordDuringExternalLogin { get; set; }

    public List<ExternalProviderSettings> ExternalProviders { get; set; } = null!;
}
