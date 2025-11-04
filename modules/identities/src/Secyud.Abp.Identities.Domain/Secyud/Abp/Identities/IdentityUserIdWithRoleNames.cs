namespace Secyud.Abp.Identities;

public class IdentityUserIdWithRoleNames
{
    public Guid Id { get; set; }

    public string[] RoleNames { get; set; } = null!;
}