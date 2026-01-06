using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Secyud.Abp.Authorization;

[Dependency(ReplaceServices = true)]
public class AbpAuthorizationService(
    IAuthorizationPolicyProvider policyProvider,
    IAuthorizationHandlerProvider handlers,
    ILogger<DefaultAuthorizationService> logger,
    IAuthorizationHandlerContextFactory contextFactory,
    IAuthorizationEvaluator evaluator,
    IOptions<AuthorizationOptions> options,
    ICurrentPrincipalAccessor currentPrincipalAccessor,
    IServiceProvider serviceProvider)
    : DefaultAuthorizationService(policyProvider, handlers, logger, contextFactory, evaluator, options)
        , IAbpAuthorizationService, ITransientDependency
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    public ClaimsPrincipal CurrentPrincipal => currentPrincipalAccessor.Principal;
}