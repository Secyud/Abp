using Secyud.Abp.Identities;

namespace Secyud.Abp.Accounts.Phone;

public interface IAccountPhoneService
{
    Task SendConfirmationCodeAsync(IdentityUser user, string confirmationToken);

    Task SendSecurityCodeAsync(IdentityUser user, string code);
}
