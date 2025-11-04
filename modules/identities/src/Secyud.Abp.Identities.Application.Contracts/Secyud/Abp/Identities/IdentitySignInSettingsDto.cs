namespace Secyud.Abp.Identities;

public class IdentitySignInSettingsDto
{
    public bool RequireConfirmedEmail { get; set; }

    public bool EnablePhoneNumberConfirmation { get; set; }

    public bool RequireConfirmedPhoneNumber { get; set; }
}