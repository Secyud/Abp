namespace Secyud.Abp.Account.ExternalProviders;

public class ExternalProviderDefinition
{
    public string Name { get; set; } = string.Empty;

    public List<ExternalProviderDefinitionProperty> Properties { get; set; } = [];
}
