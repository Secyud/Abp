using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Security.Claims;

namespace Secyud.Abp.Authorization;

public class AlwaysAllowAuthorizationService(
    IServiceProvider serviceProvider,
    ICurrentPrincipalAccessor currentPrincipalAccessor)
    : IAbpAuthorizationService
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    public ClaimsPrincipal CurrentPrincipal => currentPrincipalAccessor.Principal;

    public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
    {
        return Task.FromResult(AuthorizationResult.Success());
    }

    public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
    {
        return Task.FromResult(AuthorizationResult.Success());
    }
}
