using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.SecurityLog;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities;

public class IdentitySecurityLogManager(
    ISecurityLogManager securityLogManager,
    IdentityUserManager userManager,
    ICurrentPrincipalAccessor currentPrincipalAccessor,
    IUserClaimsPrincipalFactory<IdentityUser> userClaimsPrincipalFactory,
    ICurrentUser currentUser)
    : ITransientDependency
{
    protected ISecurityLogManager SecurityLogManager { get; } = securityLogManager;
    protected IdentityUserManager UserManager { get; } = userManager;
    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; } = currentPrincipalAccessor;
    protected IUserClaimsPrincipalFactory<IdentityUser> UserClaimsPrincipalFactory { get; } = userClaimsPrincipalFactory;
    protected ICurrentUser CurrentUser { get; } = currentUser;

    public async Task SaveAsync(IdentitySecurityLogContext context)
    {
        Action<SecurityLogInfo> securityLogAction = securityLog =>
        {
            securityLog.Identity = context.Identity;
            securityLog.Action = context.Action;

            if (!context.UserName.IsNullOrWhiteSpace())
            {
                securityLog.UserName = context.UserName;
            }

            if (!context.ClientId.IsNullOrWhiteSpace())
            {
                securityLog.ClientId = context.ClientId;
            }

            foreach (var property in context.ExtraProperties)
            {
                securityLog.ExtraProperties[property.Key] = property.Value;
            }
        };

        if (CurrentUser.IsAuthenticated)
        {
            await SecurityLogManager.SaveAsync(securityLogAction);
        }
        else
        {
            if (context.UserName.IsNullOrWhiteSpace())
            {
                await SecurityLogManager.SaveAsync(securityLogAction);
            }
            else
            {
                var user = await UserManager.FindByNameAsync(context.UserName);
                if (user != null)
                {
                    using (CurrentPrincipalAccessor.Change(await UserClaimsPrincipalFactory.CreateAsync(user)))
                    {
                        await SecurityLogManager.SaveAsync(securityLogAction);
                    }
                }
                else
                {
                    await SecurityLogManager.SaveAsync(securityLogAction);
                }
            }
        }
    }
}
