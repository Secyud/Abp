using Secyud.Abp.Permissions.Integration;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Permissions;

[Dependency(TryRegister = true)]
public class HttpClientPermissionFinder(IPermissionIntegrationService permissionIntegrationService) : IPermissionFinder, ITransientDependency
{
    protected IPermissionIntegrationService PermissionIntegrationService { get; } = permissionIntegrationService;

    public virtual async Task<List<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> requests)
    {
        return (await PermissionIntegrationService.IsGrantedAsync(requests)).Items.ToList();
    }
}
