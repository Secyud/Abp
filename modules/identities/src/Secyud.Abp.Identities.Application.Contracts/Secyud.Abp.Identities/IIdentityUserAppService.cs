using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Secyud.Abp.Identities;

public interface IIdentityUserAppService : ICrudAppService<IdentityUserDto, Guid, GetIdentityUsersInput, IdentityUserCreateDto, IdentityUserUpdateDto>
{
    Task<IdentityUserDto?> FindByIdAsync(Guid id);

    Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id);

    Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync();

    Task<List<ClaimTypeDto>> GetAllClaimTypesAsync();

    Task<List<IdentityUserClaimDto>> GetClaimsAsync(Guid id);

    Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input);

    Task UpdateClaimsAsync(Guid id, List<IdentityUserClaimDto> input);

    Task UpdatePasswordAsync(Guid id, IdentityUserUpdatePasswordInput input);

    Task LockAsync(Guid id, DateTime lockoutEnd);

    Task UnlockAsync(Guid id);

    Task<IdentityUserDto?> FindByUsernameAsync(string username);

    Task<IdentityUserDto?> FindByEmailAsync(string email);

    Task<bool> GetTwoFactorEnabledAsync(Guid id);

    Task SetTwoFactorEnabledAsync(Guid id, bool enabled);

    Task<List<IdentityRoleLookupDto>> GetRoleLookupAsync();

    Task<List<ExternalLoginProviderDto>> GetExternalLoginProvidersAsync();

    Task<IdentityUserDto> ImportExternalUserAsync(ImportExternalUserInput input);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetIdentityUserListAsFileInput input);

    Task<IRemoteStreamContent> GetListAsCsvFileAsync(GetIdentityUserListAsFileInput input);

    Task<DownloadTokenResultDto> GetDownloadTokenAsync();

    Task<IRemoteStreamContent> GetImportUsersSampleFileAsync(GetImportUsersSampleFileInput input);

    Task<ImportUsersFromFileOutput> ImportUsersFromFileAsync(ImportUsersFromFileInputWithStream input);

    Task<IRemoteStreamContent> GetImportInvalidUsersFileAsync(GetImportInvalidUsersFileInput input);
}