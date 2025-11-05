using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Secyud.Abp.Account.Localization;
using Secyud.Abp.Account.Settings;
using Secyud.Abp.Accounts.Emailing;
using Secyud.Abp.Accounts.Phone;
using Secyud.Abp.Accounts.Security.Recaptcha;
using Secyud.Abp.Identities;
using Secyud.Abp.Identities.Settings;
using Secyud.Abp.Settings;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Imaging;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Settings;
using Volo.Abp.Users;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Accounts;

public class AccountAppService : ApplicationService, IAccountAppService
{
    protected IIdentityRoleRepository RoleRepository { get; }
    protected IIdentitySecurityLogRepository SecurityLogRepository { get; }
    protected IdentityUserManager UserManager { get; }
    protected IAccountEmailer AccountEmailer { get; }
    protected IAccountPhoneService PhoneService { get; }
    protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
    public IAbpRecaptchaValidatorFactory RecaptchaValidatorFactory { get; set; }
    protected ISettingManager SettingManager { get; }
    protected IBlobContainer<AccountProfilePictureContainer> AccountProfilePictureContainer { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    protected IImageCompressor ImageCompressor { get; }
    protected IOptions<AbpProfilePictureOptions> ProfilePictureOptions { get; }
    protected IApplicationInfoAccessor ApplicationInfoAccessor { get; }
    protected IdentityUserTwoFactorChecker IdentityUserTwoFactorChecker { get; }

    public AccountAppService(
        IdentityUserManager userManager,
        IAccountEmailer accountEmailer,
        IAccountPhoneService phoneService,
        IIdentityRoleRepository roleRepository,
        IdentitySecurityLogManager identitySecurityLogManager,
        IBlobContainer<AccountProfilePictureContainer> accountProfilePictureContainer,
        ISettingManager settingManager,
        IOptions<IdentityOptions> identityOptions,
        IIdentitySecurityLogRepository securityLogRepository,
        IImageCompressor imageCompressor,
        IOptions<AbpProfilePictureOptions> profilePictureOptions,
        IApplicationInfoAccessor applicationInfoAccessor,
        IdentityUserTwoFactorChecker identityUserTwoFactorChecker)
    {
        RoleRepository = roleRepository;
        IdentitySecurityLogManager = identitySecurityLogManager;
        UserManager = userManager;
        AccountEmailer = accountEmailer;
        PhoneService = phoneService;
        AccountProfilePictureContainer = accountProfilePictureContainer;
        SettingManager = settingManager;
        IdentityOptions = identityOptions;
        SecurityLogRepository = securityLogRepository;
        ImageCompressor = imageCompressor;
        ProfilePictureOptions = profilePictureOptions;
        ApplicationInfoAccessor = applicationInfoAccessor;
        IdentityUserTwoFactorChecker = identityUserTwoFactorChecker;

        LocalizationResource = typeof(AbpAccountsResource);
        RecaptchaValidatorFactory = NullAbpRecaptchaValidatorFactory.Instance;
    }

    public virtual async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
    {
        await CheckSelfRegistrationAsync();

        if (await UseCaptchaOnRegistration())
        {
            var reCaptchaValidator = await RecaptchaValidatorFactory.CreateAsync();
            await reCaptchaValidator.ValidateAsync(input.CaptchaResponse);
        }

        await IdentityOptions.SetAsync();

        var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress, CurrentTenant.Id);

        input.MapExtraPropertiesTo(user);

        (await UserManager.CreateAsync(user, input.Password)).CheckErrors();
        (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();

        if (!user.EmailConfirmed)
        {
            await SendEmailConfirmationTokenAsync(user, input.AppName, input.ReturnUrl, input.ReturnUrlHash);
        }

        await SettingManager.SetForUserAsync(user.Id, LocalizationSettingNames.DefaultLanguage,
            $"{CultureInfo.CurrentCulture.Name};{CultureInfo.CurrentUICulture.Name}");

        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
    }

    public virtual async Task SendPasswordResetCodeAsync(SendPasswordResetCodeDto input)
    {
        var user = await UserManager.FindByEmailAsync(input.Email);
        if (user == null)
        {
            if (await SettingProvider.IsTrueAsync(AccountSettingNames.PreventEmailEnumeration))
            {
                return;
            }

            throw new UserFriendlyException(L["Secyud.Accounts:InvalidEmailAddress", input.Email]);
        }

        var resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);
        await AccountEmailer.SendPasswordResetLinkAsync(user, resetToken, input.AppName, input.ReturnUrl, input.ReturnUrlHash);
    }

    public virtual async Task<bool> VerifyPasswordResetTokenAsync(VerifyPasswordResetTokenInput input)
    {
        var user = await UserManager.GetByIdAsync(input.UserId);
        return await UserManager.VerifyUserTokenAsync(
            user,
            UserManager.Options.Tokens.PasswordResetTokenProvider,
            UserManager<IdentityUser>.ResetPasswordTokenPurpose,
            input.ResetToken);
    }

    public virtual async Task ResetPasswordAsync(ResetPasswordDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(input.UserId);
        (await UserManager.ResetPasswordAsync(user, input.ResetToken, input.Password)).CheckErrors();

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.ChangePassword
        });
    }

    public virtual async Task<IdentityUserConfirmationStateDto> GetConfirmationStateAsync(Guid id)
    {
        var user = await UserManager.GetByIdAsync(id);

        return new IdentityUserConfirmationStateDto
        {
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            EmailConfirmed = user.EmailConfirmed
        };
    }

    public virtual async Task SendPhoneNumberConfirmationTokenAsync(SendPhoneNumberConfirmationTokenDto input)
    {
        await CheckIfPhoneNumberConfirmationEnabledAsync();

        var user = await UserManager.GetByIdAsync(input.UserId);

        if (!input.PhoneNumber.IsNullOrWhiteSpace())
        {
            (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
        }

        CheckPhoneNumber(user);

        var token = await UserManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber!);
        await PhoneService.SendConfirmationCodeAsync(user, token);
    }

    public virtual async Task SendEmailConfirmationTokenAsync(SendEmailConfirmationTokenDto input)
    {
        var user = await UserManager.GetByIdAsync(input.UserId);
        await SendEmailConfirmationTokenAsync(user, input.AppName, input.ReturnUrl, input.ReturnUrlHash);
    }

    public virtual async Task<bool> VerifyEmailConfirmationTokenAsync(VerifyEmailConfirmationTokenInput input)
    {
        var user = await UserManager.GetByIdAsync(input.UserId);
        return await UserManager.VerifyUserTokenAsync(
            user,
            UserManager.Options.Tokens.EmailConfirmationTokenProvider,
            UserManager<IdentityUser>.ConfirmEmailTokenPurpose,
            input.Token);
    }

    protected virtual async Task SendEmailConfirmationTokenAsync(
        IdentityUser user,
        string applicationName,
        string? returnUrl,
        string? returnUrlHash)
    {
        var confirmationToken = await UserManager.GenerateEmailConfirmationTokenAsync(user);
        await AccountEmailer.SendEmailConfirmationLinkAsync(user, confirmationToken, applicationName, returnUrl, returnUrlHash);
    }

    public virtual async Task ConfirmPhoneNumberAsync(ConfirmPhoneNumberInput input)
    {
        await CheckIfPhoneNumberConfirmationEnabledAsync();

        var user = await UserManager.GetByIdAsync(input.UserId);

        CheckPhoneNumber(user);

        (await UserManager.ChangePhoneNumberAsync(user, user.PhoneNumber!, input.Token)).CheckErrors();

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.ChangePhoneNumber
        });
    }

    public virtual async Task ConfirmEmailAsync(ConfirmEmailInput input)
    {
        var user = await UserManager.GetByIdAsync(input.UserId);
        if (user.EmailConfirmed)
        {
            return;
        }

        (await UserManager.ConfirmEmailAsync(user, input.Token)).CheckErrors();
        (await UserManager.UpdateSecurityStampAsync(user)).CheckErrors();

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.ChangeEmail
        });
    }

    [Authorize]
    public virtual async Task SetProfilePictureAsync(ProfilePictureInput input)
    {
        await SettingManager.SetForUserAsync(CurrentUser.GetId(), AccountSettingNames.ProfilePictureSource, input.Type.ToString());

        var userIdText = CurrentUser.GetId().ToString();

        if (input.Type != ProfilePictureType.Image)
        {
            if (await AccountProfilePictureContainer.ExistsAsync(userIdText))
            {
                await AccountProfilePictureContainer.DeleteAsync(userIdText);
            }
        }
        else
        {
            if (input.ImageContent == null)
            {
                throw new NoImageProvidedException();
            }

            var imageStream = input.ImageContent.GetStream();

            if (ProfilePictureOptions.Value.EnableImageCompression)
            {
                try
                {
                    var compressResult = await ImageCompressor.CompressAsync(imageStream);

                    if (imageStream != compressResult.Result && compressResult.Result.CanRead)
                    {
                        await imageStream.DisposeAsync();
                        imageStream = compressResult.Result;
                    }
                }
                catch (Exception e)
                {
                    Logger.LogWarning(e, "Profile picture compression failed! User ID: {UserId}", CurrentUser.GetId());
                }
            }

            await AccountProfilePictureContainer.SaveAsync(userIdText, imageStream, true);
        }
    }

    public virtual async Task<ProfilePictureSourceDto> GetProfilePictureAsync(Guid id)
    {
        var pictureSource = await SettingManager.GetOrNullForUserAsync(AccountSettingNames.ProfilePictureSource, id);

        if (pictureSource == ProfilePictureType.Gravatar.ToString())
        {
            var user = await UserManager.GetByIdAsync(id);
            var gravatar = $"https://secure.gravatar.com/avatar/{GetGravatarHash(user.Email)}";
            return new ProfilePictureSourceDto
            {
                Type = ProfilePictureType.Gravatar,
                Source = gravatar,
                FileContent = await GetAvatarFromAvatarAsync(gravatar)
            };
        }

        if (pictureSource == ProfilePictureType.Image.ToString() && await AccountProfilePictureContainer.ExistsAsync(id.ToString()))
        {
            return new ProfilePictureSourceDto
            {
                Type = ProfilePictureType.Image,
                FileContent = await AccountProfilePictureContainer.GetAllBytesAsync(id.ToString())
            };
        }

        return new ProfilePictureSourceDto
        {
            Type = ProfilePictureType.None,
            FileContent = await GetDefaultAvatarAsync()
        };
    }

    public virtual async Task<IRemoteStreamContent> GetProfilePictureFileAsync(Guid id)
    {
        var picture = await GetProfilePictureAsync(id);
        return new RemoteStreamContent(new MemoryStream(picture.FileContent), contentType: "image/jpeg", disposeStream: true);
    }

    public virtual async Task<List<string>> GetTwoFactorProvidersAsync(GetTwoFactorProvidersInput input)
    {
        var user = await UserManager.GetByIdAsync(input.UserId);
        if (await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, nameof(SignInResult.RequiresTwoFactor), input.Token))
        {
            var providers = (await UserManager.GetValidTwoFactorProvidersAsync(user)).ToList();
            if (!user.HasAuthenticator())
            {
                providers.RemoveAll(x => x == TwoFactorProviderConsts.Authenticator);
            }

            return providers;
        }

        throw new UserFriendlyException(L["Secyud.Accounts:InvalidUserToken"]);
    }

    public virtual async Task SendTwoFactorCodeAsync(SendTwoFactorCodeInput input)
    {
        var user = await UserManager.GetByIdAsync(input.UserId);
        if (await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, nameof(SignInResult.RequiresTwoFactor), input.Token))
        {
            switch (input.Provider)
            {
                case TwoFactorProviderConsts.Email:
                {
                    var code = await UserManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
                    await AccountEmailer.SendEmailSecurityCodeAsync(user, code);
                    return;
                }
                case TwoFactorProviderConsts.Phone:
                {
                    var code = await UserManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
                    await PhoneService.SendSecurityCodeAsync(user, code);
                    return;
                }
                case TwoFactorProviderConsts.Authenticator:
                {
                    // No need to send code. The client will use the TOTP generator.
                    return;
                }

                default:
                    throw new UserFriendlyException(L["Secyud.Accounts:UnsupportedTwoFactorProvider"]);
            }
        }

        throw new UserFriendlyException(L["Secyud.Accounts:InvalidUserToken"]);
    }

    [Authorize]
    public virtual async Task<PagedResultDto<IdentitySecurityLogDto>> GetSecurityLogListAsync(GetIdentitySecurityLogListInput input)
    {
        var securityLogs = await SecurityLogRepository.GetListAsync(
            sorting: input.Sorting,
            maxResultCount: input.MaxResultCount,
            skipCount: input.SkipCount,
            startTime: input.StartTime,
            endTime: input.EndTime,
            applicationName: input.ApplicationName,
            identity: input.Identity,
            action: input.Action,
            userId: CurrentUser.GetId(),
            userName: input.UserName,
            clientId: input.ClientId,
            correlationId: input.CorrelationId
        );

        var totalCount = await SecurityLogRepository.GetCountAsync(
            startTime: input.StartTime,
            endTime: input.EndTime,
            applicationName: input.ApplicationName,
            identity: input.Identity,
            action: input.Action,
            userId: CurrentUser.GetId(),
            userName: input.UserName,
            clientId: input.ClientId,
            correlationId: input.CorrelationId
        );

        var securityLogDtos = ObjectMapper.Map<List<IdentitySecurityLog>, List<IdentitySecurityLogDto>>(securityLogs);
        return new PagedResultDto<IdentitySecurityLogDto>(totalCount, securityLogDtos);
    }

    [Authorize]
    public virtual async Task<VerifyAuthenticatorCodeDto> VerifyAuthenticatorCodeAsync(VerifyAuthenticatorCodeInput input)
    {
        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());
        var verificationCode = input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);
        if (await UserManager.VerifyTwoFactorTokenAsync(user, UserManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode))
        {
            user.SetAuthenticator(true);
            (await UserManager.UpdateAsync(user)).CheckErrors();
            var recoveryCodes = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            if (recoveryCodes != null)
            {
                return new VerifyAuthenticatorCodeDto
                {
                    RecoveryCodes = recoveryCodes.ToList()
                };
            }
        }

        throw new UserFriendlyException(L["Secyud.Accounts:InvalidUserToken"]);
    }

    [Authorize]
    public virtual async Task ResetAuthenticatorAsync()
    {
        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());
        await UserManager.ResetAuthenticatorKeyAsync(user);
        await UserManager.ResetRecoveryCodesAsync(user);
        user.SetAuthenticator(false);
        (await UserManager.UpdateAsync(user)).CheckErrors();
        await IdentityUserTwoFactorChecker.CheckAsync(user);
    }

    [Authorize]
    public virtual async Task<bool> HasAuthenticatorAsync()
    {
        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());
        return user.HasAuthenticator();
    }

    [Authorize]
    public virtual async Task<AuthenticatorInfoDto> GetAuthenticatorInfoAsync()
    {
        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());
        var email = await UserManager.GetEmailAsync(user);


        var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(unformattedKey))
        {
            await UserManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
        }

        var key = AuthenticatorHelper.FormatKey(unformattedKey);
        var uri = AuthenticatorHelper.GenerateQrCodeUri(email, unformattedKey, ApplicationInfoAccessor.ApplicationName);

        return new AuthenticatorInfoDto
        {
            Key = key,
            Uri = uri
        };
    }

    protected virtual string GetGravatarHash(string emailAddress)
    {
        var encodedPassword = new UTF8Encoding().GetBytes(emailAddress);

        var hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")!).ComputeHash(encodedPassword);

        return BitConverter.ToString(hash)
            .Replace("-", string.Empty)
            .ToLower();
    }

    protected virtual async Task CheckSelfRegistrationAsync()
    {
        if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled))
        {
            throw new UserFriendlyException(L["Secyud.Accounts:SelfRegistrationDisabled"]);
        }
    }

    protected virtual void CheckPhoneNumber(IdentityUser user)
    {
        if (string.IsNullOrEmpty(user.PhoneNumber))
        {
            throw new BusinessException("Secyud.Accounts:PhoneNumberEmpty");
        }
    }

    protected virtual async Task CheckIfPhoneNumberConfirmationEnabledAsync()
    {
        if (!await SettingProvider.IsTrueAsync(IdentitiesSettingNames.SignIn.EnablePhoneNumberConfirmation))
        {
            throw new BusinessException("Secyud.Accounts:PhoneNumberConfirmationDisabled");
        }
    }

    protected virtual async Task<bool> UseCaptchaOnRegistration()
    {
        return await SettingProvider.IsTrueAsync(AccountSettingNames.Captcha.UseCaptchaOnRegistration);
    }

    //TODO: cache byte[]
    protected virtual async Task<byte[]> GetAvatarFromAvatarAsync(string url)
    {
        var httpclient = LazyServiceProvider.LazyGetRequiredService<IHttpClientFactory>().CreateClient();
        var responseMessage = await httpclient.GetAsync(url);
        return await responseMessage.Content.ReadAsByteArrayAsync();
    }

    //TODO: cache byte[]
    protected virtual async Task<byte[]> GetDefaultAvatarAsync()
    {
        var virtualFileProvider = LazyServiceProvider.LazyGetRequiredService<IVirtualFileProvider>();
        await using var stream = virtualFileProvider.GetFileInfo("/Volo/Abp/Account/ProfilePictures/avatar.jpg").CreateReadStream();
        return await stream.GetAllBytesAsync();
    }
}