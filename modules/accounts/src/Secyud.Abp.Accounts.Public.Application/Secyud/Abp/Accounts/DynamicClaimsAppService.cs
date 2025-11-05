using Microsoft.AspNetCore.Authorization;
using Secyud.Abp.Identities;
using Volo.Abp.Application.Services;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace Secyud.Abp.Accounts;

[Authorize]
public class DynamicClaimsAppService(
    IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
    IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory,
    ICurrentPrincipalAccessor principalAccessor)
    : ApplicationService, IDynamicClaimsAppService
{
    protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache { get; } = identityDynamicClaimsPrincipalContributorCache;
    protected IAbpClaimsPrincipalFactory AbpClaimsPrincipalFactory { get; } = abpClaimsPrincipalFactory;
    protected ICurrentPrincipalAccessor PrincipalAccessor { get; } = principalAccessor;

    public virtual async Task RefreshAsync()
    {
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(CurrentUser.GetId(), CurrentUser.TenantId);
        await AbpClaimsPrincipalFactory.CreateDynamicAsync(PrincipalAccessor.Principal);
    }
}
