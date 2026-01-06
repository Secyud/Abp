using System.Security.Claims;

namespace Secyud.Abp.Authorization.Permissions;

public class AlwaysAllowPermissionChecker : IPermissionChecker
{
    public Task<PermissionGrantResult> IsGrantedAsync(string name)
    {
        return IsGrantedAsync(null, name);
    }

    public Task<PermissionGrantResult> IsGrantedAsync(ClaimsPrincipal? claimsPrincipal, string name)
    {
        return Task.FromResult(PermissionGrantResult.Granted);
    }

    public Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names)
    {
        return IsGrantedAsync(null, names);
    }

    public Task<MultiplePermissionGrantResult> IsGrantedAsync(ClaimsPrincipal? claimsPrincipal, string[] names)
    {
        return Task.FromResult(new MultiplePermissionGrantResult(names, PermissionGrantResult.Granted));
    }
}