using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Identities;

public class IdentityUserTwoFactorChecker(IdentityUserManager userManager) : ITransientDependency
{
    protected IdentityUserManager UserManager { get; } = userManager;

    public virtual async Task<bool> CanEnabledAsync(IdentityUser user)
    {
        var validTwoFactorProviders = await UserManager.GetValidTwoFactorProvidersAsync(user);

        if (validTwoFactorProviders.Count == 0 ||
            (validTwoFactorProviders.Count == 1 && validTwoFactorProviders.Contains(TwoFactorProviderConsts.Authenticator) && !user.HasAuthenticator()))
        {
            return false;
        }

        return true;
    }

    public virtual async Task CheckAsync(IdentityUser user)
    {
        if (!await CanEnabledAsync(user))
        {
            (await UserManager.SetTwoFactorEnabledAsync(user, false)).CheckErrors();
        }
    }
}
