using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Accounts;

public class VerifyPasswordResetTokenInput
{
    public Guid UserId { get; set; }

    [Required]
    public string ResetToken { get; set; } = "";
}