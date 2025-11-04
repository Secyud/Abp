using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace Secyud.Abp.Identities;

[RemoteService(Name = IdentitiesRemoteServiceConsts.RemoteServiceName)]
[Area(IdentitiesRemoteServiceConsts.ModuleName)]
[ControllerName("User")]
[Route("api/identities/users")]
public class IdentityUserController(IIdentityUserAppService userAppService) : AbpControllerBase, IIdentityUserAppService
{
    protected IIdentityUserAppService UserAppService { get; } = userAppService;

    [HttpGet]
    [Route("{id:guid}")]
    public virtual Task<IdentityUserDto> GetAsync(Guid id)
    {
        return UserAppService.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
    {
        return UserAppService.GetListAsync(input);
    }

    [HttpPost]
    public virtual Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
    {
        return UserAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public virtual Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
    {
        return UserAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return UserAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("by-id/{id:guid}")]
    public virtual Task<IdentityUserDto?> FindByIdAsync(Guid id)
    {
        return UserAppService.FindByIdAsync(id);
    }

    [HttpGet]
    [Route("{id:guid}/roles")]
    public virtual Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
    {
        return UserAppService.GetRolesAsync(id);
    }

    [HttpGet]
    [Route("assignable-roles")]
    public Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync()
    {
        return UserAppService.GetAssignableRolesAsync();
    }
    
    [HttpGet]
    [Route("all-claim-types")]
    public virtual Task<List<ClaimTypeDto>> GetAllClaimTypesAsync()
    {
        return UserAppService.GetAllClaimTypesAsync();
    }

    [HttpGet]
    [Route("{id:guid}/claims")]
    public virtual Task<List<IdentityUserClaimDto>> GetClaimsAsync(Guid id)
    {
        return UserAppService.GetClaimsAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/roles")]
    public virtual Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
    {
        return UserAppService.UpdateRolesAsync(id, input);
    }

    [HttpPut]
    [Route("{id:guid}/claims")]
    public virtual Task UpdateClaimsAsync(Guid id, List<IdentityUserClaimDto> input)
    {
        return UserAppService.UpdateClaimsAsync(id, input);
    }

    [HttpPut]
    [Route("{id:guid}/lock/{lockoutEnd:datetime}")]
    public Task LockAsync(Guid id, DateTime lockoutEnd)
    {
        return UserAppService.LockAsync(id, lockoutEnd);
    }

    [HttpPut]
    [Route("{id:guid}/unlock")]
    public virtual Task UnlockAsync(Guid id)
    {
        return UserAppService.UnlockAsync(id);
    }

    [HttpGet]
    [Route("by-username/{username}")]
    public virtual Task<IdentityUserDto?> FindByUsernameAsync(string username)
    {
        return UserAppService.FindByUsernameAsync(username);
    }

    [HttpGet]
    [Route("by-email/{email}")]
    public virtual Task<IdentityUserDto?> FindByEmailAsync(string email)
    {
        return UserAppService.FindByEmailAsync(email);
    }

    [HttpGet]
    [Route("{id:guid}/two-factor-enabled")]
    public Task<bool> GetTwoFactorEnabledAsync(Guid id)
    {
        return UserAppService.GetTwoFactorEnabledAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/two-factor/{enabled:bool}")]
    public Task SetTwoFactorEnabledAsync(Guid id, bool enabled)
    {
        return UserAppService.SetTwoFactorEnabledAsync(id, enabled);
    }

    [HttpPut]
    [Route("{id:guid}/change-password")]
    public virtual Task UpdatePasswordAsync(Guid id, IdentityUserUpdatePasswordInput input)
    {
        return UserAppService.UpdatePasswordAsync(id, input);
    }

    [HttpGet]
    [Route("lookup/roles")]
    public virtual Task<List<IdentityRoleLookupDto>> GetRoleLookupAsync()
    {
        return UserAppService.GetRoleLookupAsync();
    }

    [HttpGet]
    [Route("external-login-Providers")]
    public virtual Task<List<ExternalLoginProviderDto>> GetExternalLoginProvidersAsync()
    {
        return UserAppService.GetExternalLoginProvidersAsync();
    }

    [HttpPost]
    [Route("import-external-user")]
    public Task<IdentityUserDto> ImportExternalUserAsync(ImportExternalUserInput input)
    {
        return UserAppService.ImportExternalUserAsync(input);
    }

    [HttpGet]
    [Route("export-as-excel")]
    [AllowAnonymous]
    public Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetIdentityUserListAsFileInput input)
    {
        return UserAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    [Route("export-as-csv")]
    [AllowAnonymous]
    public Task<IRemoteStreamContent> GetListAsCsvFileAsync(GetIdentityUserListAsFileInput input)
    {
        return UserAppService.GetListAsCsvFileAsync(input);
    }

    [HttpGet]
    [Route("download-token")]
    public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return UserAppService.GetDownloadTokenAsync();
    }

    [HttpGet]
    [Route("import-users-sample-file")]
    public Task<IRemoteStreamContent> GetImportUsersSampleFileAsync(GetImportUsersSampleFileInput input)
    {
        return UserAppService.GetImportUsersSampleFileAsync(input);
    }

    [HttpPost]
    [Route("import-users-from-file")]
    public Task<ImportUsersFromFileOutput> ImportUsersFromFileAsync(ImportUsersFromFileInputWithStream input)
    {
        return UserAppService.ImportUsersFromFileAsync(input);
    }

    [HttpGet]
    [Route("download-import-invalid-users-file")]
    public Task<IRemoteStreamContent> GetImportInvalidUsersFileAsync(GetImportInvalidUsersFileInput input)
    {
        return UserAppService.GetImportInvalidUsersFileAsync(input);
    }
}
