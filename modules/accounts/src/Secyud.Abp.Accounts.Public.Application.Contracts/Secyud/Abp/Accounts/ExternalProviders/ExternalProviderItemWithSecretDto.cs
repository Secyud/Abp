using Secyud.Abp.Account.ExternalProviders;

namespace Secyud.Abp.Accounts.ExternalProviders;

public class ExternalProviderItemWithSecretDto
{
    public bool Success { get; set; }

    public string? Name { get; set; }

    public bool Enabled { get; set; }

    public List<ExternalProviderSettingsProperty>? Properties { get; set; }

    public List<ExternalProviderSettingsProperty>? SecretProperties { get; set; }
}
