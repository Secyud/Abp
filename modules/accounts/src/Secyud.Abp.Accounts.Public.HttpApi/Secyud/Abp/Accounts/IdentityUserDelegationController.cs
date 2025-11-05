using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Accounts;

[RemoteService(Name = AccountProPublicRemoteServiceConsts.RemoteServiceName)]
[Area(AccountProPublicRemoteServiceConsts.ModuleName)]
[ControllerName("User")]
[Route("api/account/user-delegation")]
public class IdentityUserDelegationController: AbpControllerBase, IIdentityUserDelegationAppService
{
    protected IIdentityUserDelegationAppService IdentityUserDelegationAppService { get; }
    
    public IdentityUserDelegationController(IIdentityUserDelegationAppService identityUserDelegationAppService)
    {
        IdentityUserDelegationAppService = identityUserDelegationAppService;
    }
    
    [HttpGet]
    [Route("delegated-users")]
    public virtual Task<ListResultDto<UserDelegationDto>> GetDelegatedUsersAsync()
    {
        return IdentityUserDelegationAppService.GetDelegatedUsersAsync();
    }

    [HttpGet]
    [Route("my-delegated-users")]
    public virtual Task<ListResultDto<UserDelegationDto>> GetMyDelegatedUsersAsync()
    {
        return IdentityUserDelegationAppService.GetMyDelegatedUsersAsync();
    }
    
    [HttpGet]
    [Route("active-delegations")]
    public virtual Task<ListResultDto<UserDelegationDto>> GetActiveDelegationsAsync()
    {
        return IdentityUserDelegationAppService.GetActiveDelegationsAsync();
    }

    [HttpGet]
    [Route("user-lookup")]
    public virtual Task<ListResultDto<UserLookupDto>> GetUserLookupAsync(GetUserLookupInput input)
    {
        return IdentityUserDelegationAppService.GetUserLookupAsync(input);
    }

    [HttpPost]
    [Route("delegate-new-user")]
    public virtual Task DelegateNewUserAsync(DelegateNewUserInput input)
    {
        return IdentityUserDelegationAppService.DelegateNewUserAsync(input);
    }

    [HttpPost]
    [Route("delete-delegation")]
    public virtual Task DeleteDelegationAsync(Guid id)
    {
        return IdentityUserDelegationAppService.DeleteDelegationAsync(id);
    }
}