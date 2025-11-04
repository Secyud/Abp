using Volo.Abp.Data;

namespace Secyud.Abp.Identities;


public static class IdentityUserExtensions
{
    public static IdentityUser SetAuthenticator(this IdentityUser user, bool authenticator)
    {
        user.SetProperty(TwoFactorProviderConsts.Authenticator, authenticator);
        return user;
    }

    public static bool HasAuthenticator(this IdentityUser user)
    {
        return user.GetProperty<bool>(TwoFactorProviderConsts.Authenticator);
    }
}
