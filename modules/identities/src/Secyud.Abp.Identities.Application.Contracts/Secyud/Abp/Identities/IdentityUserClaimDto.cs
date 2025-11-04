namespace Secyud.Abp.Identities;

public class IdentityUserClaimDto
{
    public Guid UserId { get; set; }

    public string ClaimType { get; set; } = string.Empty;

    public string ClaimValue { get; set; } = string.Empty;
}