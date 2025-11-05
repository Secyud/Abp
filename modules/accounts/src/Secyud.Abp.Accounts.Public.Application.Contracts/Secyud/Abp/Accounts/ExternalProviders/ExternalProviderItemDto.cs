using Secyud.Abp.Account.ExternalProviders;

namespace Secyud.Abp.Accounts.ExternalProviders;

public class ExternalProviderItemDto
{
    public required string Name { get; set; }

    public bool Enabled { get; set; }

    public required List<ExternalProviderSettingsProperty> Properties { get; set; }
}
