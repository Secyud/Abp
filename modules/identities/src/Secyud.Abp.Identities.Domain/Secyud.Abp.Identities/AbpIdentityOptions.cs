namespace Secyud.Abp.Identities;

public class AbpIdentityOptions
{
    public ExternalLoginProviderDictionary ExternalLoginProviders { get; } = new();
}
