using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Accounts;

public class SendPhoneSecurityCodeDto
{
    [Required]
    public Guid UserId { get; set; }
}
