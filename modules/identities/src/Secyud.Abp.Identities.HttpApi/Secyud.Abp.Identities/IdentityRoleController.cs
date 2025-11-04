using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Identities;

[RemoteService(Name = IdentitiesRemoteServiceConsts.RemoteServiceName)]
[Area(IdentitiesRemoteServiceConsts.ModuleName)]
[ControllerName("Role")]
[Route("api/identities/roles")]
public class IdentityRoleController(IIdentityRoleAppService roleAppService) : AbpControllerBase, IIdentityRoleAppService
{
    protected IIdentityRoleAppService RoleAppService { get; } = roleAppService;

    [HttpGet]
    [Route("{id}")]
    public virtual Task<IdentityRoleDto> GetAsync(Guid id)
    {
        return RoleAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input)
    {
        return RoleAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
    {
        return RoleAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return RoleAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("all")]
    public virtual Task<ListResultDto<IdentityRoleDto>> GetAllListAsync()
    {
        return RoleAppService.GetAllListAsync();
    }

    [HttpGet]
    [Route("")]
    public virtual Task<PagedResultDto<IdentityRoleDto>> GetListAsync(GetIdentityRoleListInput input)
    {
        return RoleAppService.GetListAsync(input);
    }

    [HttpPut]
    [Route("{id}/claims")]
    public virtual Task UpdateClaimsAsync(Guid id, List<IdentityRoleClaimDto> input)
    {
        return RoleAppService.UpdateClaimsAsync(id, input);
    }

    [HttpGet]
    [Route("{id}/claims")]
    public virtual Task<List<IdentityRoleClaimDto>> GetClaimsAsync(Guid id)
    {
        return RoleAppService.GetClaimsAsync(id);
    }

    [HttpPut]
    [Route("{id}/move-all-users")]
    public virtual Task MoveAllUsersAsync(Guid id, [FromQuery]Guid? roleId)
    {
        return RoleAppService.MoveAllUsersAsync(id, roleId);
    }

    [HttpGet]
    [Route("all-claim-types")]
    public virtual Task<List<ClaimTypeDto>> GetAllClaimTypesAsync()
    {
        return RoleAppService.GetAllClaimTypesAsync();
    }
}
