using System.Security.Claims;

namespace Secyud.Abp.Authorization.Permissions;

public interface IPermissionChecker
{
    Task<PermissionGrantResult> IsGrantedAsync(string name);
    Task<PermissionGrantResult> IsGrantedAsync(ClaimsPrincipal? claimsPrincipal, string name);
    Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names);
    Task<MultiplePermissionGrantResult> IsGrantedAsync(ClaimsPrincipal? claimsPrincipal, string[] names);
}