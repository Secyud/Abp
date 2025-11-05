namespace Secyud.Abp.Accounts;

public class AccountSettingsDto
{
    public bool IsSelfRegistrationEnabled { get; set; }

    public bool EnableLocalLogin { get; set; }

    public bool PreventEmailEnumeration { get; set; }
}
