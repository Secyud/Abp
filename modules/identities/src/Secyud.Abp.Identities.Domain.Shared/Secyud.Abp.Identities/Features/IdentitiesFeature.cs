namespace Secyud.Abp.Identities.Features;

public class IdentitiesFeature
{
    public const string GroupName = "Identity";

    public const string TwoFactor = GroupName + ".TwoFactor";

    public const string MaxUserCount = GroupName + ".MaxUserCount";
    
    public const string EnableLdapLogin = "Account.EnableLdapLogin";
    
    public const string EnableOAuthLogin = GroupName + ".EnableOAuthLogin";
    
}
