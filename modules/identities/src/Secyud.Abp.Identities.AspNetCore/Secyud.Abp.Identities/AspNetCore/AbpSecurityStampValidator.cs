using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Secyud.Abp.Identities.AspNetCore;

public class AbpSecurityStampValidator(
    IOptions<SecurityStampValidatorOptions> options,
    SignInManager<IdentityUser> signInManager,
    ILoggerFactory loggerFactory,
    ITenantConfigurationProvider tenantConfigurationProvider,
    ICurrentTenant currentTenant)
    : SecurityStampValidator<IdentityUser>(options,
        signInManager,
        loggerFactory)
{
    protected ITenantConfigurationProvider TenantConfigurationProvider { get; } = tenantConfigurationProvider;
    protected ICurrentTenant CurrentTenant { get; } = currentTenant;

    [UnitOfWork]
    public override async Task ValidateAsync(CookieValidatePrincipalContext context)
    {
        TenantConfiguration? tenant = null;
        try
        {
            tenant = await TenantConfigurationProvider.GetAsync(saveResolveResult: false);
        }
        catch (Exception e)
        {
            Logger.LogException(e);
        }

        using (CurrentTenant.Change(tenant?.Id, tenant?.Name))
        {
            await base.ValidateAsync(context);
        }
    }
}
