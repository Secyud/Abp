using Secyud.Abp.Authorization;
using Volo.Abp;

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

        public async Task<AuthorizationResult> AuthorizeAsync(object resource,
            IEnumerable<IAuthorizationRequirement> requirements)
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

        private IAbpAuthorizationService AsAbpAuthorizationService()
        {
            return authorizationService as IAbpAuthorizationService ?? throw new AbpException(
                $"{nameof(authorizationService)} should implement {typeof(IAbpAuthorizationService).FullName}");
        }
    }
}