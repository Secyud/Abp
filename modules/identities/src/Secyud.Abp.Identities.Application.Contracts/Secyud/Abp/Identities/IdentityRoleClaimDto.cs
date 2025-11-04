namespace Secyud.Abp.Identities;

public class IdentityRoleClaimDto
{
    public Guid RoleId { get; set; }

    public string ClaimType { get; set; } = string.Empty;

    public string ClaimValue { get; set; } = string.Empty;
}