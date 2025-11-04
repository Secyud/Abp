using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MiniExcelLibs;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.Csv;
using MiniExcelLibs.OpenXml;
using Secyud.Abp.Identities.Session;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Uow;

namespace Secyud.Abp.Identities;

[Authorize(IdentityPermissions.Users.DefaultName)]
public class IdentityUserAppService(
    IdentityUserManager userManager,
    IIdentityUserRepository userRepository,
    IIdentityRoleRepository roleRepository,
    IIdentityClaimTypeRepository identityClaimTypeRepository,
    IdentitiesTwoFactorManager identitiesTwoFactorManager,
    IOptions<IdentityOptions> identityOptions,
    IDistributedEventBus distributedEventBus,
    IOptions<AbpIdentityOptions> abpIdentityOptions,
    IPermissionChecker permissionChecker,
    IDistributedCache<IdentityUserDownloadTokenCacheItem, string> downloadTokenCache,
    IDistributedCache<ImportInvalidUsersCacheItem, string> importInvalidUsersCache,
    IdentitySessionManager identitySessionManager,
    IdentityUserTwoFactorChecker identityUserTwoFactorChecker)
    : IdentityAppServiceBase, IIdentityUserAppService
{
    protected IdentityUserManager UserManager { get; } = userManager;
    protected IIdentityUserRepository UserRepository { get; } = userRepository;
    protected IIdentityRoleRepository RoleRepository { get; } = roleRepository;
    protected IIdentityClaimTypeRepository IdentityClaimTypeRepository { get; } = identityClaimTypeRepository;
    protected IdentitiesTwoFactorManager IdentitiesTwoFactorManager { get; } = identitiesTwoFactorManager;
    protected IOptions<IdentityOptions> IdentityOptions { get; } = identityOptions;
    protected IOptions<AbpIdentityOptions> AbpIdentityOptions { get; } = abpIdentityOptions;
    protected IDistributedEventBus DistributedEventBus { get; } = distributedEventBus;
    protected IPermissionChecker PermissionChecker { get; } = permissionChecker;
    protected IDistributedCache<IdentityUserDownloadTokenCacheItem, string> DownloadTokenCache { get; } = downloadTokenCache;
    protected IDistributedCache<ImportInvalidUsersCacheItem, string> ImportInvalidUsersCache { get; } = importInvalidUsersCache;
    protected IdentitySessionManager IdentitySessionManager { get; } = identitySessionManager;
    protected IdentityUserTwoFactorChecker IdentityUserTwoFactorChecker { get; } = identityUserTwoFactorChecker;

    public virtual async Task<IdentityUserDto> GetAsync(Guid id)
    {
        var userDto = await FindByIdInternalAsync(id);

        if (userDto == null)
        {
            throw new EntityNotFoundException(typeof(IdentityUser), id);
        }

        return userDto;
    }

    public virtual async Task<IdentityUserDto?> FindByIdAsync(Guid id)
    {
        return await FindByIdInternalAsync(id);
    }

    public virtual async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
    {
        var count = await UserRepository.GetCountAsync(
            input.Filter,
            input.RoleId,
            input.OrganizationUnitId,
            input.UserName,
            input.PhoneNumber,
            input.EmailAddress,
            input.Name,
            input.Surname,
            input.IsLockedOut,
            input.NotActive,
            input.EmailConfirmed,
            input.IsExternal,
            input.MaxCreationTime,
            input.MinCreationTime,
            input.MaxModifitionTime,
            input.MinModifitionTime
        );

        var users = await UserRepository.GetListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.Filter,
            includeDetails: false,
            input.RoleId,
            input.OrganizationUnitId,
            input.UserName,
            input.PhoneNumber,
            input.EmailAddress,
            input.Name,
            input.Surname,
            input.IsLockedOut,
            input.NotActive,
            input.EmailConfirmed,
            input.IsExternal,
            input.MaxCreationTime,
            input.MinCreationTime,
            input.MaxModifitionTime,
            input.MinModifitionTime
        );
        var userDtos = await FillIdentityUserDtoAsync(UserRepository, IdentitiesTwoFactorManager, users);
        return new PagedResultDto<IdentityUserDto>(
            count,
            userDtos
        );
    }

    public virtual async Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
    {
        var roles = await UserRepository.GetRolesAsync(id);
        return new ListResultDto<IdentityRoleDto>(
            ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(roles)
        );
    }

    public virtual async Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync()
    {
        var list = await RoleRepository.GetListAsync();
        return new ListResultDto<IdentityRoleDto>(
            ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list));
    }

    public virtual async Task<List<ClaimTypeDto>> GetAllClaimTypesAsync()
    {
        var claimTypes = await IdentityClaimTypeRepository.GetListAsync();

        var dtos = ObjectMapper.Map<List<IdentityClaimType>, List<ClaimTypeDto>>(claimTypes);
        foreach (var dto in dtos)
        {
            dto.ValueTypeAsString = dto.ValueType.ToString();
        }

        return dtos;
    }

    public virtual async Task<List<IdentityUserClaimDto>> GetClaimsAsync(Guid id)
    {
        var user = await UserRepository.GetAsync(id);
        return
        [
            ..ObjectMapper.Map<List<IdentityUserClaim>, List<IdentityUserClaimDto>>(user.Claims.ToList())
        ];
    }

    [Authorize(IdentityPermissions.Users.CreateName)]
    public virtual async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
    {
        await IdentityOptions.SetAsync();

        var user = new IdentityUser(
            GuidGenerator.Create(),
            input.UserName,
            input.Email,
            CurrentTenant.Id
        )
        {
            Surname = input.Surname,
            Name = input.Name
        };

        user.SetPhoneNumber(input.PhoneNumber, false);
        user.SetEmailConfirmed(input.EmailConfirmed);
        user.SetPhoneNumberConfirmed(input.PhoneNumberConfirmed);

        input.MapExtraPropertiesTo(user);

        (await UserManager.CreateAsync(user, input.Password)).CheckErrors();
        await UpdateUserByInput(user, input);
        (await UserManager.UpdateAsync(user)).CheckErrors();
        await CurrentUnitOfWork!.SaveChangesAsync();

        await DistributedEventBus.PublishAsync(new IdentityUserCreatedEto()
        {
            Id = user.Id,
            Properties =
            {
                { "SendConfirmationEmail", input.SendConfirmationEmail.ToString().ToUpper() },
                { "AppName", "MVC" }
            }
        });

        var userDto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);

        return userDto;
    }

    [Authorize(IdentityPermissions.Users.UpdateName)]
    public virtual async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(id);

        user.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();
        await UpdateUserByInput(user, input);

        user.SetEmailConfirmed(input.EmailConfirmed);
        user.SetPhoneNumberConfirmed(input.PhoneNumberConfirmed);

        input.MapExtraPropertiesTo(user);
        (await UserManager.UpdateAsync(user)).CheckErrors();
        await CurrentUnitOfWork!.SaveChangesAsync();

        var userDto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);

        return userDto;
    }

    [Authorize(IdentityPermissions.Users.DeleteName)]
    public virtual async Task DeleteAsync(Guid id)
    {
        if (CurrentUser.Id == id)
        {
            throw new BusinessException(code: IdentitiesErrorCodes.UserSelfDeletion);
        }

        var user = await UserManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return;
        }

        (await UserManager.DeleteAsync(user)).CheckErrors();
        await IdentitySessionManager.RevokeAllAsync(user.Id);
    }

    [Authorize(IdentityPermissions.Users.UpdateName)]
    public virtual async Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
    {
        await IdentityOptions.SetAsync();
        var user = await UserManager.GetByIdAsync(id);
        (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
        await UserRepository.UpdateAsync(user);
    }

    [Authorize(IdentityPermissions.Users.UpdateName)]
    public virtual async Task UpdateClaimsAsync(Guid id, List<IdentityUserClaimDto> input)
    {
        var user = await UserRepository.GetAsync(id);

        foreach (var claim in input)
        {
            var existing = user.FindClaim(new Claim(claim.ClaimType, claim.ClaimValue));
            if (existing == null)
            {
                user.AddClaim(GuidGenerator, new Claim(claim.ClaimType, claim.ClaimValue));
            }
        }

        //Copied with ToList to avoid modification of the collection in the for loop
        foreach (var claim in user.Claims.ToList())
        {
            if (!input.Any(c => claim.ClaimType == c.ClaimType && claim.ClaimValue == c.ClaimValue))
            {
                user.RemoveClaim(new Claim(claim.ClaimType, claim.ClaimValue));
            }
        }

        if (user.Claims.Count != 0)
        {
            var claimNames = user.Claims.Select(x => x.ClaimType);
            var claims = await IdentityClaimTypeRepository.GetListByNamesAsync(claimNames);

            foreach (var claim in claims.Where(x => x.ValueType == IdentityClaimValueType.String))
            {
                var userClaim = user.Claims.FirstOrDefault(x => x.ClaimType == claim.Name);
                if (userClaim == null)
                {
                    continue;
                }

                if (claim.Required && userClaim.ClaimValue.IsNullOrWhiteSpace())
                {
                    throw new UserFriendlyException(L["ClaimValueCanNotBeBlank"]);
                }

                if (!claim.Regex.IsNullOrWhiteSpace() && !Regex.IsMatch(userClaim.ClaimValue, claim.Regex, RegexOptions.None, TimeSpan.FromSeconds(1)))
                {
                    throw new UserFriendlyException(L["ClaimValueIsInvalid", claim.Name]);
                }
            }
        }

        await UserRepository.UpdateAsync(user);
    }

    [Authorize(IdentityPermissions.Users.UpdateName)]
    public virtual async Task LockAsync(Guid id, DateTime lockoutEnd)
    {
        var user = await UserManager.GetByIdAsync(id);
        if (!await UserManager.GetLockoutEnabledAsync(user))
        {
            throw new UserFriendlyException(L["UserLockoutNotEnabled{0}", user.UserName!]);
        }

        (await UserManager.SetLockoutEndDateAsync(user, lockoutEnd.ToUniversalTime())).CheckErrors();
        await IdentitySessionManager.RevokeAllAsync(user.Id);
    }

    [Authorize(IdentityPermissions.Users.UpdateName)]
    public virtual async Task UnlockAsync(Guid id)
    {
        var user = await UserManager.GetByIdAsync(id);
        if (!await UserManager.GetLockoutEnabledAsync(user))
        {
            throw new UserFriendlyException(L["UserLockoutNotEnabled{0}", user.UserName!]);
        }

        (await UserManager.SetLockoutEndDateAsync(user, null)).CheckErrors();
    }

    [Authorize(IdentityPermissions.Users.UpdateName)]
    public virtual async Task UpdatePasswordAsync(Guid id, IdentityUserUpdatePasswordInput input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(id);
        (await UserManager.RemovePasswordAsync(user)).CheckErrors();
        (await UserManager.AddPasswordAsync(user, input.NewPassword)).CheckErrors();
    }

    public virtual async Task<IdentityUserDto?> FindByUsernameAsync(string username)
    {
        var user = await UserManager.FindByNameAsync(username);

        var userDto = ObjectMapper.Map<IdentityUser?, IdentityUserDto?>(user);

        return userDto;
    }

    public virtual async Task<IdentityUserDto?> FindByEmailAsync(string email)
    {
        var user = await UserManager.FindByEmailAsync(email);

        var userDto = ObjectMapper.Map<IdentityUser?, IdentityUserDto?>(user);

        return userDto;
    }

    public virtual async Task<bool> GetTwoFactorEnabledAsync(Guid id)
    {
        var user = await UserManager.GetByIdAsync(id);
        return await UserManager.GetTwoFactorEnabledAsync(user);
    }

    [Authorize(IdentityPermissions.Users.UpdateName)]
    public virtual async Task SetTwoFactorEnabledAsync(Guid id, bool enabled)
    {
        if (await IdentitiesTwoFactorManager.IsOptionalAsync())
        {
            var user = await UserManager.GetByIdAsync(id);
            if (user.TwoFactorEnabled != enabled)
            {
                if (enabled)
                {
                    if (!await IdentityUserTwoFactorChecker.CanEnabledAsync(user))
                    {
                        throw new UserFriendlyException(L["YouCanNotEnableTwoFactorForThisUser"]);
                    }
                }

                (await UserManager.SetTwoFactorEnabledAsync(user, enabled)).CheckErrors();
            }
        }
        else
        {
            throw new BusinessException(code: IdentitiesErrorCodes.CanNotChangeTwoFactor);
        }
    }

    public virtual async Task<List<IdentityRoleLookupDto>> GetRoleLookupAsync()
    {
        var roles = await RoleRepository.GetListAsync();

        return ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleLookupDto>>(roles);
    }


    [Authorize(IdentityPermissions.Users.ImportName)]
    public virtual async Task<List<ExternalLoginProviderDto>> GetExternalLoginProvidersAsync()
    {
        var providers = new List<ExternalLoginProviderDto>();

        foreach (var externalLoginProvider in AbpIdentityOptions.Value.ExternalLoginProviders)
        {
            var provider = LazyServiceProvider.LazyGetRequiredService(externalLoginProvider.Value.Type).As<IExternalLoginProvider>();

            if (await provider.IsEnabledAsync())
            {
                var canObtainUserInfoWithoutPassword = true;
                if (provider is IExternalLoginProviderWithPassword providerWithPassword)
                {
                    canObtainUserInfoWithoutPassword = providerWithPassword.CanObtainUserInfoWithoutPassword;
                }

                providers.Add(new ExternalLoginProviderDto(externalLoginProvider.Value.Name, canObtainUserInfoWithoutPassword));
            }
        }

        return providers;
    }

    [Authorize(IdentityPermissions.Users.ImportName)]
    public virtual async Task<IdentityUserDto> ImportExternalUserAsync(ImportExternalUserInput input)
    {
        if (!AbpIdentityOptions.Value.ExternalLoginProviders.TryGetValue(input.Provider, out var providerInfo))
        {
            throw new BusinessException(IdentitiesErrorCodes.InvalidExternalLoginProvider);
        }

        var provider = LazyServiceProvider.LazyGetRequiredService(providerInfo.Type).As<IExternalLoginProvider>();
        var user = await UserManager.FindByNameAsync(input.UserNameOrEmailAddress) ?? await UserManager.FindByEmailAsync(input.UserNameOrEmailAddress);

        if (provider is IExternalLoginProviderWithPassword)
        {
            if (!provider.As<IExternalLoginProviderWithPassword>().CanObtainUserInfoWithoutPassword &&
                !await provider.TryAuthenticateAsync(input.UserNameOrEmailAddress, input.Password))
            {
                throw new BusinessException(IdentitiesErrorCodes.ExternalLoginProviderAuthenticateFailed);
            }
        }

        if (user == null)
        {
            if (provider is IExternalLoginProviderWithPassword providerWithPassword)
            {
                user = await providerWithPassword.CreateUserAsync(input.UserNameOrEmailAddress, input.Provider, input.Password);
            }
            else
            {
                user = await provider.CreateUserAsync(input.UserNameOrEmailAddress, input.Provider);
            }
        }
        else
        {
            if (!user.IsExternal)
            {
                throw new BusinessException(IdentitiesErrorCodes.LocalUserAlreadyExists);
            }

            if (provider is IExternalLoginProviderWithPassword providerWithPassword)
            {
                await providerWithPassword.UpdateUserAsync(user, input.Provider, input.Password);
            }
            else
            {
                await provider.UpdateUserAsync(user, input.Provider);
            }
        }

        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetIdentityUserListAsFileInput input)
    {
        var userDtos = await GetExportUsersAsync(input);

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(userDtos, excelType: ExcelType.XLSX, configuration: GetExportUsersConfig(ExcelType.XLSX));
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "UserList.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsCsvFileAsync(GetIdentityUserListAsFileInput input)
    {
        var userDtos = await GetExportUsersAsync(input);

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(userDtos, excelType: ExcelType.CSV, configuration: GetExportUsersConfig(ExcelType.CSV));
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "UserList.csv", "text/csv");
    }

    public virtual async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await DownloadTokenCache.SetAsync(
            token,
            new IdentityUserDownloadTokenCacheItem { Token = token, TenantId = CurrentTenant.Id },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

        return new DownloadTokenResultDto
        {
            Token = token
        };
    }

    [Authorize(IdentityPermissions.Users.ImportName)]
    public virtual async Task<ImportUsersFromFileOutput> ImportUsersFromFileAsync(ImportUsersFromFileInputWithStream input)
    {
        await IdentityOptions.SetAsync();

        var stream = new MemoryStream();
        await input.File.GetStream().CopyToAsync(stream);

        var invalidUsers = new List<InvalidImportUsersFromFileDto>();
        List<InvalidImportUsersFromFileDto> waitingImportUsers;
        try
        {
            IConfiguration? configuration = null;
            if (input.FileType == ImportUsersFromFileType.Csv)
            {
                configuration = new CsvConfiguration { Seperator = ';' };
            }

            waitingImportUsers =
                (await stream.QueryAsync<InvalidImportUsersFromFileDto>(
                    excelType: input.FileType == ImportUsersFromFileType.Excel ? ExcelType.XLSX : ExcelType.CSV, configuration: configuration)).ToList();
        }
        catch (Exception)
        {
            throw new BusinessException(IdentitiesErrorCodes.InvalidImportFileFormat);
        }

        if (!waitingImportUsers.Any())
        {
            throw new BusinessException(IdentitiesErrorCodes.NoUserFoundInFile);
        }

        var resultDto = new ImportUsersFromFileOutput
        {
            AllCount = waitingImportUsers.Count
        };

        foreach (var waitingImportUser in waitingImportUsers)
        {
            using var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true);
            try
            {
                var user = new IdentityUser(
                    GuidGenerator.Create(),
                    waitingImportUser.UserName,
                    waitingImportUser.EmailAddress,
                    CurrentTenant.Id
                )
                {
                    Surname = waitingImportUser.Surname,
                    Name = waitingImportUser.Name
                };

                if (!waitingImportUser.PhoneNumber.IsNullOrWhiteSpace())
                {
                    user.SetPhoneNumber(waitingImportUser.PhoneNumber, false);
                }

                if (!waitingImportUser.Password.IsNullOrWhiteSpace())
                {
                    (await UserManager.CreateAsync(user, waitingImportUser.Password)).CheckErrors();
                }
                else
                {
                    (await UserManager.CreateAsync(user)).CheckErrors();
                }

                if (!waitingImportUser.AssignedRoleNames.IsNullOrWhiteSpace())
                {
                    (await UserManager.SetRolesAsync(user, waitingImportUser.AssignedRoleNames.Split(";")
                        .Select(role => role.Trim())
                        .Where(role => !role.IsNullOrWhiteSpace()))).CheckErrors();
                }

                await uow.CompleteAsync();
            }
            catch (Exception e)
            {
                waitingImportUser.ErrorReason = e is UserFriendlyException ? e.Message : e.ToString();

                invalidUsers.Add(waitingImportUser);
                Logger.LogWarning(e, "Import user failed: {InvalidImportUsersFromFileDto}", waitingImportUser);

                await uow.RollbackAsync();
            }
        }

        if (invalidUsers.Any())
        {
            var token = Guid.NewGuid().ToString("N");

            await ImportInvalidUsersCache.SetAsync(
                token,
                new ImportInvalidUsersCacheItem
                {
                    Token = token,
                    InvalidUsers = invalidUsers,
                    FileType = input.FileType
                },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                });

            resultDto.InvalidUsersDownloadToken = token;
        }

        resultDto.SucceededCount = resultDto.AllCount - invalidUsers.Count;
        resultDto.FailedCount = invalidUsers.Count;

        return resultDto;
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetImportUsersSampleFileAsync(GetImportUsersSampleFileInput input)
    {
        await CheckDownloadTokenAsync(input.Token);

        var sampleUsers = new List<ImportUsersFromFileDto>
        {
            new()
            {
                UserName = "JohnDoe",
                Name = "John",
                Surname = "Doe",
                EmailAddress = "john.doe@acme.com",
                PhoneNumber = "123456789",
                Password = "3j9A9g*",
                AssignedRoleNames = "admin"
            },
            new()
            {
                UserName = "DouglasAdams42",
                Name = "Douglas Noel",
                Surname = "Adams",
                EmailAddress = "douglas.n.adams@gmail.com",
                PhoneNumber = "6165435434",
                Password = "jurQ892*",
                AssignedRoleNames = "admin"
            }
        };

        return await GetImportUsersFileAsync(sampleUsers, "ImportUsersSampleFile", input.FileType);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetImportInvalidUsersFileAsync(GetImportInvalidUsersFileInput input)
    {
        await CheckDownloadTokenAsync(input.Token, isInvalidUsersToken: true);

        var invalidUsersCacheItem = await ImportInvalidUsersCache.GetAsync(input.Token);
        if (invalidUsersCacheItem is null)
        {
            throw new UserFriendlyException("Import user invalid file get failed");
        }

        return await GetImportUsersFileAsync(invalidUsersCacheItem.InvalidUsers, "InvalidUsers", invalidUsersCacheItem.FileType);
    }

    protected virtual async Task<IRemoteStreamContent> GetImportUsersFileAsync<T>(
        List<T> users,
        string fileName,
        ImportUsersFromFileType fileType) where T : ImportUsersFromFileDto
    {
        string contentType;
        ExcelType excelType;
        IConfiguration? configuration = null;
        switch (fileType)
        {
            case ImportUsersFromFileType.Excel:
            default:
                fileName += ".xlsx";
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                excelType = ExcelType.XLSX;
                break;
            case ImportUsersFromFileType.Csv:
                fileName += ".csv";
                contentType = "text/csv";
                excelType = ExcelType.CSV;
                configuration = new CsvConfiguration { Seperator = ';' };
                break;
        }

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(users, excelType: excelType, configuration: configuration);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, fileName, contentType);
    }

    protected virtual async Task<List<IdentityUserExportDto>> GetExportUsersAsync(GetIdentityUserListAsFileInput input)
    {
        var downloadToken = await CheckDownloadTokenAsync(input.Token);

        using (CurrentTenant.Change(downloadToken.TenantId))
        {
            var users = await UserRepository.GetListAsync(
                input.Sorting,
                int.MaxValue,
                0,
                input.Filter,
                includeDetails: false,
                input.RoleId,
                input.OrganizationUnitId,
                input.UserName,
                input.PhoneNumber,
                input.EmailAddress,
                input.Name,
                input.Surname,
                input.IsLockedOut,
                input.NotActive,
                input.EmailConfirmed,
                input.IsExternal,
                input.MaxCreationTime,
                input.MinCreationTime,
                input.MaxModifitionTime,
                input.MinModifitionTime
            );

            var userIds = users.Select(x => x.Id);

            var userRoles = await UserRepository.GetRoleNamesAsync(userIds);

            var userDtos = ObjectMapper.Map<List<IdentityUser>, List<IdentityUserExportDto>>(users);

            for (var i = 0; i < users.Count; i++)
            {
                var userRole = userRoles.FirstOrDefault(x => x.Id == users[i].Id);
                if (userRole != null)
                {
                    userDtos[i].Roles = userRole.RoleNames.JoinAsString(";");
                }
            }

            return userDtos;
        }
    }

    protected virtual async Task<IDownloadCacheItem> CheckDownloadTokenAsync(string token, bool isInvalidUsersToken = false)
    {
        IDownloadCacheItem? downloadToken;
        if (isInvalidUsersToken)
        {
            downloadToken = await ImportInvalidUsersCache.GetAsync(token);
        }
        else
        {
            downloadToken = await DownloadTokenCache.GetAsync(token);
        }

        if (downloadToken == null || token != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + token);
        }

        return downloadToken;
    }

    protected virtual async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
    {
        if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
        }

        if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
        }

        (await UserManager.SetLockoutEnabledAsync(user, input.LockoutEnabled)).CheckErrors();

        user.Name = input.Name?.Trim();
        user.Surname = input.Surname?.Trim();
        (await UserManager.UpdateAsync(user)).CheckErrors();
        if (user.Id != CurrentUser.Id)
        {
            user.SetIsActive(input.IsActive);
            if (!user.IsActive)
            {
                await IdentitySessionManager.RevokeAllAsync(user.Id);
            }
        }

        user.SetShouldChangePasswordOnNextLogin(input.ShouldChangePasswordOnNextLogin);

        if (await PermissionChecker.IsGrantedAsync(IdentityPermissions.Users.ManageRoles.Name) && input.RoleNames != null)
        {
            (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
        }
    }


    protected virtual async Task<IdentityUserDto?> FindByIdInternalAsync(Guid id)
    {
        var user = await UserManager.FindByIdAsync(id.ToString());
        var userDto = ObjectMapper.Map<IdentityUser?, IdentityUserDto?>(user);

        if (user == null)
        {
            return userDto;
        }

        userDto!.RoleNames = (await UserManager.GetRolesAsync(user)).ToList();
        userDto.SupportTwoFactor = await IdentitiesTwoFactorManager.IsOptionalAsync();

        return userDto;
    }

    private IConfiguration GetExportUsersConfig(ExcelType excelType)
    {
        var dynamicColumns = new DynamicExcelColumn[]
        {
            new("UserName") { Name = "User name", Width = 15 },
            new("Email") { Name = "Email address", Width = 20 },
            new("Roles") { Width = 10 },
            new("PhoneNumber") { Name = "Phone number", Width = 15 },
            new("Name") { Width = 10 },
            new("Surname") { Width = 10 },
            new("IsActive") { Name = "Active", Width = 10 },
            new("AccountLookout") { Name = "Account lookout", Width = 15 },
            new("EmailConfirmed") { Name = "Email confirmed", Width = 15 },
            new("TwoFactorEnabled") { Name = "Two factor enabled", Width = 15 },
            new("AccessFailedCount") { Name = "Access failed count", Width = 15 },
            new("CreationTime") { Name = "Creation time", Width = 15 },
            new("LastModificationTime") { Name = "Last modification time", Width = 20 }
        };

        switch (excelType)
        {
            case ExcelType.XLSX:
            case ExcelType.UNKNOWN:
            default:
                return new OpenXmlConfiguration
                {
                    DynamicColumns = dynamicColumns
                };
            case ExcelType.CSV:
                return new CsvConfiguration
                {
                    DynamicColumns = dynamicColumns,
                    Seperator = ';'
                };
        }
    }
}