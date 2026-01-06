using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Authorization;

public interface IAbpAuthorizationService : IAuthorizationService, IServiceProviderAccessor
{
    ClaimsPrincipal CurrentPrincipal { get; }
}