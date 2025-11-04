using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities;

[RemoteService(Name = IdentitiesRemoteServiceConsts.RemoteServiceName)]
[Area(IdentitiesRemoteServiceConsts.ModuleName)]
[ControllerName("UserLookup")]
[Route("api/identities/users/lookup")]
[Obsolete("Use IdentityUserIntegrationController for module-to-module (or service-to-service) communication.")]
public class IdentityUserLookupController(IIdentityUserLookupAppService lookupAppService) : AbpControllerBase, IIdentityUserLookupAppService
{
    protected IIdentityUserLookupAppService LookupAppService { get; } = lookupAppService;

    [HttpGet]
    [Route("{id:guid}")]
    public virtual Task<UserData?> FindByIdAsync(Guid id)
    {
        return LookupAppService.FindByIdAsync(id);
    }

    [HttpGet]
    [Route("by-username/{userName}")]
    public virtual Task<UserData?> FindByUserNameAsync(string userName)
    {
        return LookupAppService.FindByUserNameAsync(userName);
    }

    [HttpGet]
    [Route("search")]
    public Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input)
    {
        return LookupAppService.SearchAsync(input);
    }

    [HttpGet]
    [Route("count")]
    public Task<long> GetCountAsync(UserLookupCountInputDto input)
    {
        return LookupAppService.GetCountAsync(input);
    }
}