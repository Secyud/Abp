using Secyud.Abp.Identities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Secyud.Abp.Accounts;

public interface IAccountAppService : IApplicationService
{
    Task<IdentityUserDto> RegisterAsync(RegisterDto input);

    Task SendPasswordResetCodeAsync(SendPasswordResetCodeDto input);

    Task<bool> VerifyPasswordResetTokenAsync(VerifyPasswordResetTokenInput input);

    Task ResetPasswordAsync(ResetPasswordDto input);

    Task<IdentityUserConfirmationStateDto> GetConfirmationStateAsync(Guid id);

    Task SendPhoneNumberConfirmationTokenAsync(SendPhoneNumberConfirmationTokenDto input);

    Task SendEmailConfirmationTokenAsync(SendEmailConfirmationTokenDto input);

    Task<bool> VerifyEmailConfirmationTokenAsync(VerifyEmailConfirmationTokenInput input);

    Task ConfirmPhoneNumberAsync(ConfirmPhoneNumberInput input);

    Task ConfirmEmailAsync(ConfirmEmailInput input);

    Task SetProfilePictureAsync(ProfilePictureInput input);

    Task<ProfilePictureSourceDto> GetProfilePictureAsync(Guid id);

    Task<IRemoteStreamContent> GetProfilePictureFileAsync(Guid id);

    Task<List<string>> GetTwoFactorProvidersAsync(GetTwoFactorProvidersInput input);

    Task SendTwoFactorCodeAsync(SendTwoFactorCodeInput input);

    Task<PagedResultDto<IdentitySecurityLogDto>> GetSecurityLogListAsync(GetIdentitySecurityLogListInput input);

    Task<VerifyAuthenticatorCodeDto> VerifyAuthenticatorCodeAsync(VerifyAuthenticatorCodeInput input);

    Task ResetAuthenticatorAsync();

    Task<bool> HasAuthenticatorAsync();

    Task<AuthenticatorInfoDto> GetAuthenticatorInfoAsync();
}
