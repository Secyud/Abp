using Volo.Abp;

namespace Secyud.Abp.Identities;

public class ExternalLoginUserInfo(string email)
{
    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? PhoneNumber { get; set; }

    public string Email { get; private set; } = Check.NotNullOrWhiteSpace(email, nameof(email));

    public bool? PhoneNumberConfirmed { get; set; }

    public bool? EmailConfirmed { get; set; }

    public bool? TwoFactorEnabled { get; set; }

    public string ProviderKey { get; set; } = "";
}
