namespace Secyud.Abp.Identities;

public class IdentityUserExportDto
{
    public string? UserName { get; set; }
    
    public string? Email { get; set; }
    
    public string? Roles { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string? Name { get; set; }

    public string? Surname { get; set; }
    
    public string? Active { get; set; }
    
    public string? AccountLookout { get; set; }
    
    public string? EmailConfirmed { get; set; }

    public string? TwoFactorEnabled { get; set; }
    
    public string? AccessFailedCount { get; set; }
    
    public DateTime CreationTime { get; set; }
    
    public DateTime? LastModificationTime { get; set; }
}