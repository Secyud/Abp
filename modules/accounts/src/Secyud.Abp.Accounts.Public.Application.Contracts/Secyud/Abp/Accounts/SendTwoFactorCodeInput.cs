using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Accounts;

public class SendTwoFactorCodeInput
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string Provider { get; set; }= "";

    [Required]
    public string Token { get; set; }= "";
}
