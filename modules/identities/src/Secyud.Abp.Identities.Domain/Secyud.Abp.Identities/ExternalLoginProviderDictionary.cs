namespace Secyud.Abp.Identities;

public class ExternalLoginProviderDictionary : Dictionary<string, ExternalLoginProviderInfo>
{
    /// <summary>
    /// Adds or replaces a provider.
    /// </summary>
    public void Add<TProvider>(string name)
        where TProvider : IExternalLoginProvider
    {
        this[name] = new ExternalLoginProviderInfo(name, typeof(TProvider));
    }
}
