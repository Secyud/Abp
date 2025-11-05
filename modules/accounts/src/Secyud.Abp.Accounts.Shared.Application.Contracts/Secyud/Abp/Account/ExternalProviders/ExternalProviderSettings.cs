namespace Secyud.Abp.Account.ExternalProviders;

[Serializable]
public class ExternalProviderSettings
{
    public string Name { get; set; } = string.Empty;

    public bool Enabled { get; set; }

    public List<ExternalProviderSettingsProperty> Properties { get; set; } = [];

    public List<ExternalProviderSettingsProperty> SecretProperties { get; set; } = [];


    public bool IsValid()
    {
        return Properties.Any(x => !x.Value.IsNullOrWhiteSpace()) ||
               SecretProperties.Any(x => !x.Value.IsNullOrWhiteSpace());
    }
}