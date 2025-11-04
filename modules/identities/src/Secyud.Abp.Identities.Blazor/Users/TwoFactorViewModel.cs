namespace Secyud.Abp.Identities.Users;

public class TwoFactorViewModel
{
    public Guid Id { get; set; }

    public string? UserName { get; set; } 

    public bool TwoFactorEnabled { get; set; }
}