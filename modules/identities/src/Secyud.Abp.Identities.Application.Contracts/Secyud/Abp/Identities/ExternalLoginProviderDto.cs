namespace Secyud.Abp.Identities;

public class ExternalLoginProviderDto(string name, bool canObtainUserInfoWithoutPassword)
{
    public string Name { get; set; } = name;

    public bool CanObtainUserInfoWithoutPassword { get; set; } = canObtainUserInfoWithoutPassword;
}