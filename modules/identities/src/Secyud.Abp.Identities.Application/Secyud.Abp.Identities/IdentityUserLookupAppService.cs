using Microsoft.AspNetCore.Authorization;
using Secyud.Abp.Identities.Integration;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities;

[Obsolete("Use IdentityUserIntegrationService for module-to-module (or service-to-service) communication.")]
[Authorize(IdentityPermissions.UserLookup.DefaultName)]
public class IdentityUserLookupAppService(IIdentityUserIntegrationService identityUserIntegrationService)
    : IdentityAppServiceBase, IIdentityUserLookupAppService
{
    protected IIdentityUserIntegrationService IdentityUserIntegrationService { get; } = identityUserIntegrationService;

    public virtual async Task<UserData?> FindByIdAsync(Guid id)
    {
        return await IdentityUserIntegrationService.FindByIdAsync(id);
    }

    public virtual async Task<UserData?> FindByUserNameAsync(string userName)
    {
        return await IdentityUserIntegrationService.FindByUserNameAsync(userName);
    }

    public virtual async Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input)
    {
        return await IdentityUserIntegrationService.SearchAsync(input);
    }

    public virtual async Task<long> GetCountAsync(UserLookupCountInputDto input)
    {
        return await IdentityUserIntegrationService.GetCountAsync(input);
    }
}