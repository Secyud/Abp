using Secyud.Abp.Identities;

namespace Secyud.Abp.Accounts.Emailing;

public interface IAccountEmailer
{
    Task SendPasswordResetLinkAsync(
        IdentityUser user,
        string resetToken,
        string appName,
        string? returnUrl = null,
        string? returnUrlHash = null
    );

    Task SendEmailConfirmationLinkAsync(
        IdentityUser user,
        string confirmationToken,
        string appName,
        string? returnUrl = null,
        string? returnUrlHash = null
    );

    Task SendEmailSecurityCodeAsync(
        IdentityUser user,
        string code
    );
}
