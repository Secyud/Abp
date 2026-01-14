using Secyud.Abp.Authorization;
using Volo.Abp;
using Volo.Abp.Authorization;

namespace Microsoft.AspNetCore.Authorization;

public static class AbpAuthorizationServiceExtensions
{
    /// <param name="authorizationService">The <see cref="IAuthorizationService"/> providing authorization.</param>
    extension(IAuthorizationService authorizationService)
    {
        public async Task<AuthorizationResult> AuthorizeAsync(string policyName)
        {
            return await AuthorizeAsync(
                authorizationService,
                null,
                policyName
            );
        }

        public async Task<AuthorizationResult> AuthorizeAsync(object resource, IAuthorizationRequirement requirement)
        {
            return await authorizationService.AuthorizeAsync(
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                resource,
                requirement
            );
        }

        public async Task<AuthorizationResult> AuthorizeAsync(object? resource, AuthorizationPolicy policy)
        {
            return await authorizationService.AuthorizeAsync(
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                resource,
                policy
            );
        }

        public async Task<AuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy)
        {
            return await AuthorizeAsync(
                authorizationService,
                null,
                policy
            );
        }

        public async Task<AuthorizationResult> AuthorizeAsync(object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return await authorizationService.AuthorizeAsync(
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                resource,
                requirements
            );
        }

        public async Task<AuthorizationResult> AuthorizeAsync(object? resource, string policyName)
        {
            return await authorizationService.AuthorizeAsync(
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                resource,
                policyName
            );
        }

        public async Task<bool> IsGrantedAsync(string policyName)
        {
            return (await authorizationService.AuthorizeAsync(policyName)).Succeeded;
        }

        public async Task<bool> IsGrantedAnyAsync(params string[] policyNames)
        {
            Check.NotNullOrEmpty(policyNames, nameof(policyNames));

            foreach (var policyName in policyNames)
            {
                if ((await authorizationService.AuthorizeAsync(policyName)).Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> IsGrantedAsync(object resource, IAuthorizationRequirement requirement)
        {
            return (await authorizationService.AuthorizeAsync(resource, requirement)).Succeeded;
        }

        public async Task<bool> IsGrantedAsync(object resource, AuthorizationPolicy policy)
        {
            return (await authorizationService.AuthorizeAsync(resource, policy)).Succeeded;
        }

        public async Task<bool> IsGrantedAsync(AuthorizationPolicy policy)
        {
            return (await authorizationService.AuthorizeAsync(policy)).Succeeded;
        }

        public async Task<bool> IsGrantedAsync(object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return (await authorizationService.AuthorizeAsync(resource, requirements)).Succeeded;
        }

        public async Task<bool> IsGrantedAsync(object resource, string policyName)
        {
            return (await authorizationService.AuthorizeAsync(resource, policyName)).Succeeded;
        }

        /// <summary>
        /// Checks if CurrentPrincipal meets a specific authorization policy, throwing an <see cref="AbpAuthorizationException"/> if not.
        /// </summary>
        /// <param name="policyName">The name of the policy to evaluate.</param>
        public async Task CheckAsync(string policyName)
        {
            if (!await authorizationService.IsGrantedAsync(policyName))
            {
                throw new AbpAuthorizationException(code:
                        AbpAuthorizationErrorCodes.GivenPolicyHasNotGrantedWithPolicyName)
                    .WithData("PolicyName", policyName);
            }
        }

        /// <summary>
        /// Checks if CurrentPrincipal meets a specific requirement for the specified resource, throwing an <see cref="AbpAuthorizationException"/> if not.
        /// </summary>
        /// <param name="resource">The resource to evaluate the policy against.</param>
        /// <param name="requirement">The requirement to evaluate the policy against.</param>
        public async Task CheckAsync(object resource, IAuthorizationRequirement requirement)
        {
            if (!await authorizationService.IsGrantedAsync(resource, requirement))
            {
                throw new AbpAuthorizationException(code:
                        AbpAuthorizationErrorCodes.GivenRequirementHasNotGrantedForGivenResource)
                    .WithData("ResourceName", resource);
            }
        }

        /// <summary>
        /// Checks if CurrentPrincipal meets a specific authorization policy against the specified resource, throwing an <see cref="AbpAuthorizationException"/> if not.
        /// </summary>
        /// <param name="resource">The resource to evaluate the policy against.</param>
        /// <param name="policy">The policy to evaluate.</param>
        public async Task CheckAsync(object resource, AuthorizationPolicy policy)
        {
            if (!await authorizationService.IsGrantedAsync(resource, policy))
            {
                throw new AbpAuthorizationException(code: 
                        AbpAuthorizationErrorCodes.GivenPolicyHasNotGrantedForGivenResource)
                    .WithData("ResourceName", resource);
            }
        }

        /// <summary>
        /// Checks if CurrentPrincipal meets a specific authorization policy, throwing an <see cref="AbpAuthorizationException"/> if not.
        /// </summary>
        /// <param name="policy">The policy to evaluate.</param>
        public async Task CheckAsync(AuthorizationPolicy policy)
        {
            if (!await authorizationService.IsGrantedAsync(policy))
            {
                throw new AbpAuthorizationException(code: 
                    AbpAuthorizationErrorCodes.GivenPolicyHasNotGranted);
            }
        }

        /// <summary>
        /// Checks if CurrentPrincipal meets a specific authorization policy against the specified resource, throwing an <see cref="AbpAuthorizationException"/> if not.
        /// </summary>
        /// <param name="resource">The resource to evaluate the policy against.</param>
        /// <param name="requirements">The requirements to evaluate the policy against.</param>
        public async Task CheckAsync(object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            if (!await authorizationService.IsGrantedAsync(resource, requirements))
            {
                throw new AbpAuthorizationException(code: 
                        AbpAuthorizationErrorCodes.GivenRequirementsHasNotGrantedForGivenResource)
                    .WithData("ResourceName", resource);
            }
        }

        /// <summary>
        /// Checks if CurrentPrincipal meets a specific authorization policy against the specified resource, throwing an <see cref="AbpAuthorizationException"/> if not.
        /// </summary>
        /// <param name="resource">The resource to evaluate the policy against.</param>
        /// <param name="policyName">The name of the policy to evaluate.</param>
        public async Task CheckAsync(object resource, string policyName)
        {
            if (!await authorizationService.IsGrantedAsync(resource, policyName))
            {
                throw new AbpAuthorizationException(code: 
                        AbpAuthorizationErrorCodes.GivenPolicyHasNotGrantedForGivenResource)
                    .WithData("ResourceName", resource);
            }
        }

        private IAbpAuthorizationService AsAbpAuthorizationService()
        {
            if (!(authorizationService is IAbpAuthorizationService abpAuthorizationService))
            {
                throw new AbpException($"{nameof(authorizationService)} should implement {typeof(IAbpAuthorizationService).FullName}");
            }

            return abpAuthorizationService;
        }
    }
}
