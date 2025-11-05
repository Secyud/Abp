using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Accounts;

public class SendPhoneNumberConfirmationTokenDto
{
    [Required]
    public Guid UserId { get; set; }

    public string? PhoneNumber { get; set; }
}
