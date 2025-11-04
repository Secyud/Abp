using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities.Integration;

[RemoteService(Name = IdentitiesRemoteServiceConsts.RemoteServiceName)]
[Area(IdentitiesRemoteServiceConsts.ModuleName)]
[ControllerName("UserIntegration")]
[Route("integration-api/identities/users")]
public class IdentityUserIntegrationController(IIdentityUserIntegrationService userIntegrationService) : AbpControllerBase, IIdentityUserIntegrationService
{
    protected IIdentityUserIntegrationService UserIntegrationService { get; } = userIntegrationService;

    [HttpGet]
    [Route("{id:guid}/role-names")]
    public virtual Task<string[]> GetRoleNamesAsync(Guid id)
    {
        return UserIntegrationService.GetRoleNamesAsync(id);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public Task<UserData?> FindByIdAsync(Guid id)
    {
        return UserIntegrationService.FindByIdAsync(id);
    }

    [HttpGet]
    [Route("by-username/{userName}")]
    public Task<UserData?> FindByUserNameAsync(string userName)
    {
        return UserIntegrationService.FindByUserNameAsync(userName);
    }

    [HttpGet]
    [Route("search")]
    public Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input)
    {
        return UserIntegrationService.SearchAsync(input);
    }

    [HttpGet]
    [Route("count")]
    public Task<long> GetCountAsync(UserLookupCountInputDto input)
    {
        return UserIntegrationService.GetCountAsync(input);
    }
}