using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Identities;

public class AbpAuthenticatorTokenProvider : AuthenticatorTokenProvider<IdentityUser>, ITransientDependency
{
    public override async Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<IdentityUser> manager, IdentityUser user)
    {
        return user.HasAuthenticator() && await base.CanGenerateTwoFactorTokenAsync(manager, user);
    }
}
