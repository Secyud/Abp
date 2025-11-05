using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Accounts;

public class ConfirmEmailInput
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string Token { get; set; }= "";
}
