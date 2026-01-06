using Microsoft.AspNetCore.Authorization;

namespace Secyud.Abp.Authorization;

public interface IAbpAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    Task<List<string>> GetPoliciesNamesAsync();
}
