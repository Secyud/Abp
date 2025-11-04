using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Identities;

[RemoteService(Name = IdentitiesRemoteServiceConsts.RemoteServiceName)]
[Area(IdentitiesRemoteServiceConsts.ModuleName)]
[ControllerName("ClaimType")]
[Route("api/identities/claim-types")]
public class IdentityClaimTypeController(IIdentityClaimTypeAppService claimTypeAppService) : AbpControllerBase, IIdentityClaimTypeAppService
{
    protected IIdentityClaimTypeAppService ClaimTypeAppService { get; } = claimTypeAppService;

    [HttpGet]
    public virtual Task<PagedResultDto<ClaimTypeDto>> GetListAsync(GetIdentityClaimTypesInput input)
    {
        return ClaimTypeAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<ClaimTypeDto> GetAsync(Guid id)
    {
        return ClaimTypeAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<ClaimTypeDto> CreateAsync(CreateClaimTypeDto input)
    {
        return ClaimTypeAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<ClaimTypeDto> UpdateAsync(Guid id, UpdateClaimTypeDto input)
    {
        return ClaimTypeAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return ClaimTypeAppService.DeleteAsync(id);
    }
}
