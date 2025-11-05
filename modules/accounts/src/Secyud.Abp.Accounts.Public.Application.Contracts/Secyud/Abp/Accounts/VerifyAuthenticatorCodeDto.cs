namespace Secyud.Abp.Accounts;

public class VerifyAuthenticatorCodeDto
{
    public required List<string> RecoveryCodes { get; set; }
}
