using System.Security.Claims;
using System.Security.Principal;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Secyud.Abp.Identities.Session;

public class IdentitySessionClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
{
    public Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        if (identity == null)
        {
            return Task.CompletedTask;
        }

        var sessionId = identity.FindSessionId();
        if (sessionId == null)
        {
            identity.AddClaim(new Claim(AbpClaimTypes.SessionId, Guid.NewGuid().ToString()));
        }

        return Task.CompletedTask;
    }
}
