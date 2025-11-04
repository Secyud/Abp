namespace Secyud.Abp.Identities.Users;

public class IdentityUserClaimViewModel
{
    public string ClaimType { get; set; } = "";

    public string? ClaimValueText { get; set; } 

    public int ClaimValueNumeric { get; set; }

    public DateTime ClaimValueDate { get; set; }

    public bool ClaimValueBool { get; set; }
}