using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Accounts;

public class LinkUserInput
{
    public Guid UserId { get; set; }

    public Guid? TenantId { get; set; }

    [Required]
    public string Token { get; set; }= "";
}
