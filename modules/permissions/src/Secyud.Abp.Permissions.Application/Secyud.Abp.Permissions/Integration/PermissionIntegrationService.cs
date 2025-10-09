using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Secyud.Abp.Permissions.Integration;

[IntegrationService]
public class PermissionIntegrationService(IPermissionFinder permissionFinder) : ApplicationService, IPermissionIntegrationService
{
    protected IPermissionFinder PermissionFinder { get; } = permissionFinder;

    public virtual async Task<ListResultDto<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> input)
    {
        return new ListResultDto<IsGrantedResponse>(await PermissionFinder.IsGrantedAsync(input));
    }
}
