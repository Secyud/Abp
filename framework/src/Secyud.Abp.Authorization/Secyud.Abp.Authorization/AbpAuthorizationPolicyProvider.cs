using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Secyud.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Authorization;

public class AbpAuthorizationPolicyProvider(
    IOptions<AuthorizationOptions> options,
    IPermissionDefinitionManager permissionDefinitionManager)
    : DefaultAuthorizationPolicyProvider(options), IAbpAuthorizationPolicyProvider, ITransientDependency
{
    private AuthorizationOptions Options { get; } = options.Value;
    private IPermissionDefinitionManager PermissionDefinitionManager { get; } = permissionDefinitionManager;

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null)
        {
            return policy;
        }

        var permission = await PermissionDefinitionManager.GetOrNullAsync(policyName);
        if (permission != null)
        {
            var policyBuilder = new AuthorizationPolicyBuilder();
            policyBuilder.Requirements.Add(new PermissionRequirement(policyName));
            return policyBuilder.Build();
        }

        return null;
    }

    public async Task<List<string>> GetPoliciesNamesAsync()
    {
        var permissionNames = await PermissionDefinitionManager.GetPermissionsAsync();

        return Options.GetPoliciesNames()
            .Union(permissionNames.Select(p => p.Name))
            .ToList();
    }
}