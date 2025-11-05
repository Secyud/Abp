namespace Secyud.Abp.Account.ExternalLogins;

[Serializable]
public class ExternalLoginSetting
{
    public virtual string LoginProvider { get; set; }

    public virtual string ProviderKey { get; set; }

    public ExternalLoginSetting(string loginProvider, string providerKey)
    {
        LoginProvider = loginProvider;
        ProviderKey = providerKey;
    }
}
