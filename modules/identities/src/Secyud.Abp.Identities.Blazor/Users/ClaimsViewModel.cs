namespace Secyud.Abp.Identities.Users;

public class ClaimsViewModel
{
    public Guid Id { get; set; }

    public string? UserName { get; set; } 

    public List<ClaimTypeDto> AllClaims { get; set; } = [];

    public List<IdentityUserClaimViewModel> OwnedClaims { get; set; } = [];
}