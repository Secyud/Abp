using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;

namespace Secyud.Abp.Identities;

public class AbpUserClaimsPrincipalFactory(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<IdentityOptions> options,
    ICurrentPrincipalAccessor currentPrincipalAccessor,
    IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory)
    : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>(userManager, roleManager, options), ITransientDependency
{
    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; } = currentPrincipalAccessor;
    protected IAbpClaimsPrincipalFactory AbpClaimsPrincipalFactory { get; } = abpClaimsPrincipalFactory;

    [UnitOfWork]
    public override async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
    {
        var principal = await base.CreateAsync(user);
        var identity = principal.Identities.First();

        if (user.TenantId.HasValue)
        {
            identity.AddIfNotContains(new Claim(AbpClaimTypes.TenantId, user.TenantId?.ToString() ?? ""));
        }

        if (!user.Name.IsNullOrWhiteSpace())
        {
            identity.AddIfNotContains(new Claim(AbpClaimTypes.Name, user.Name));
        }

        if (!user.Surname.IsNullOrWhiteSpace())
        {
            identity.AddIfNotContains(new Claim(AbpClaimTypes.SurName, user.Surname));
        }

        if (!user.PhoneNumber.IsNullOrWhiteSpace())
        {
            identity.AddIfNotContains(new Claim(AbpClaimTypes.PhoneNumber, user.PhoneNumber));
        }

        identity.AddIfNotContains(
            new Claim(AbpClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed.ToString()));

        if (!user.Email.IsNullOrWhiteSpace())
        {
            identity.AddIfNotContains(new Claim(AbpClaimTypes.Email, user.Email));
        }

        identity.AddIfNotContains(new Claim(AbpClaimTypes.EmailVerified, user.EmailConfirmed.ToString()));

        using (CurrentPrincipalAccessor.Change(identity))
        {
            await AbpClaimsPrincipalFactory.CreateAsync(principal);
        }

        return principal;
    }
}