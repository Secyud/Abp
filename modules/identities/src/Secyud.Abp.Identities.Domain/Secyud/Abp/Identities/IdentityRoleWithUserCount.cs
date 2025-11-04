namespace Secyud.Abp.Identities;

public class IdentityRoleWithUserCount(IdentityRole role, long userCount)
{
    public IdentityRole Role { get; set; } = role;

    public long UserCount { get; set; } = userCount;
}
