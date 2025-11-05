using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Secyud.Abp.Account.Localization;
using Secyud.Abp.Identities;
using Secyud.Abp.Identities.Settings;
using Secyud.Abp.Settings;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Settings;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace Secyud.Abp.Accounts;

[Authorize]
public class ProfileAppService : ApplicationService, IProfileAppService
{
    protected IdentityUserManager UserManager { get; }
    protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
    protected IdentitiesTwoFactorManager IdentityProTwoFactorManager { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    protected IdentityUserTwoFactorChecker IdentityUserTwoFactorChecker { get; }
    protected ITimezoneProvider TimezoneProvider { get; }
    protected ISettingManager SettingManager { get; }

    public ProfileAppService(
        IdentityUserManager userManager,
        IdentitySecurityLogManager identitySecurityLogManager,
        IdentitiesTwoFactorManager identityProTwoFactorManager,
        IOptions<IdentityOptions> identityOptions,
        IdentityUserTwoFactorChecker identityUserTwoFactorChecker,
        ITimezoneProvider timezoneProvider,
        ISettingManager settingManager)
    {
        UserManager = userManager;
        IdentitySecurityLogManager = identitySecurityLogManager;
        IdentityProTwoFactorManager = identityProTwoFactorManager;
        IdentityOptions = identityOptions;
        IdentityUserTwoFactorChecker = identityUserTwoFactorChecker;
        TimezoneProvider = timezoneProvider;
        SettingManager = settingManager;
        LocalizationResource = typeof(AbpAccountsResource);
    }

    public virtual async Task<ProfileDto> GetAsync()
    {
        var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());
        return await SetTimezoneInfoAsync(ObjectMapper.Map<IdentityUser, ProfileDto>(currentUser));
    }

    public virtual async Task<ProfileDto> UpdateAsync(UpdateProfileDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

        user.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        if (!string.Equals(user.UserName, input.UserName, StringComparison.InvariantCultureIgnoreCase))
        {
            if (await SettingProvider.IsTrueAsync(IdentitiesSettingNames.User.IsUserNameUpdateEnabled))
            {
                (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();
                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                {
                    Identity = IdentitySecurityLogIdentityConsts.Identity,
                    Action = IdentitySecurityLogActionConsts.ChangeUserName
                });
            }
        }

        if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
        {
            if (await SettingProvider.IsTrueAsync(IdentitiesSettingNames.User.IsEmailUpdateEnabled))
            {
                (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
                await IdentityUserTwoFactorChecker.CheckAsync(user);
                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                {
                    Identity = IdentitySecurityLogIdentityConsts.Identity,
                    Action = IdentitySecurityLogActionConsts.ChangeEmail
                });
            }
        }

        if (user.PhoneNumber.IsNullOrWhiteSpace() && input.PhoneNumber.IsNullOrWhiteSpace())
        {
            input.PhoneNumber = user.PhoneNumber;
        }

        if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
        {

            (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
            await IdentityUserTwoFactorChecker.CheckAsync(user);
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.ChangePhoneNumber
            });
        }

        user.Name = input.Name?.Trim();
        user.Surname = input.Surname?.Trim();

        input.MapExtraPropertiesTo(user);

        (await UserManager.UpdateAsync(user)).CheckErrors();

        await SettingManager.SetForCurrentUserAsync(TimingSettingNames.TimeZone, input.Timezone);

        var profileDto = await SetTimezoneInfoAsync(ObjectMapper.Map<IdentityUser, ProfileDto>(user));

        await CurrentUnitOfWork!.SaveChangesAsync();

        return profileDto;
    }

    public virtual async Task ChangePasswordAsync(ChangePasswordInput input)
    {
        await IdentityOptions.SetAsync();

        var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());

        if (currentUser.IsExternal)
        {
            throw new BusinessException(code: IdentitiesErrorCodes.ExternalUserPasswordChange);
        }

        if (currentUser.PasswordHash == null)
        {
            (await UserManager.AddPasswordAsync(currentUser, input.NewPassword)).CheckErrors();
        }
        else
        {
            (await UserManager.ChangePasswordAsync(currentUser, input.CurrentPassword, input.NewPassword)).CheckErrors();
        }

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.ChangePassword
        });
    }

    public virtual async Task<bool> GetTwoFactorEnabledAsync()
    {
        var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());
        return await UserManager.GetTwoFactorEnabledAsync(currentUser);
    }

    public virtual async Task SetTwoFactorEnabledAsync(bool enabled)
    {
        if (await IdentityProTwoFactorManager.IsOptionalAsync())
        {
            if (await SettingProvider.GetAsync<bool>(IdentitiesSettingNames.TwoFactor.UsersCanChange))
            {
                var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());
                if (currentUser.TwoFactorEnabled != enabled)
                {
                    if (enabled)
                    {
                        if (!await IdentityUserTwoFactorChecker.CanEnabledAsync(currentUser))
                        {
                            throw new UserFriendlyException(L["YouHaveToEnableAtLeastOneTwoFactorProvider"]);
                        }
                    }

                    (await UserManager.SetTwoFactorEnabledAsync(currentUser, enabled)).CheckErrors();
                }
            }
            else
            {
                throw new BusinessException(code: IdentitiesErrorCodes.UsersCanNotChangeTwoFactor);
            }
        }
        else
        {
            throw new BusinessException(code: IdentitiesErrorCodes.CanNotChangeTwoFactor);
        }
    }


    public virtual async Task<bool> CanEnableTwoFactorAsync()
    {
        var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());
        return await IdentityUserTwoFactorChecker.CanEnabledAsync(currentUser);
    }

    public virtual Task<List<NameValue>> GetTimezonesAsync()
    {
        return Task.FromResult(TimeZoneHelper.GetTimezones(TimezoneProvider.GetWindowsTimezones()));
    }

    protected virtual async Task<ProfileDto> SetTimezoneInfoAsync(ProfileDto profileDto)
    {
        profileDto.SupportsMultipleTimezone = Clock.SupportsMultipleTimezone;
        profileDto.Timezone = await SettingProvider.GetOrNullAsync(TimingSettingNames.TimeZone);
        return profileDto;
    }
}
