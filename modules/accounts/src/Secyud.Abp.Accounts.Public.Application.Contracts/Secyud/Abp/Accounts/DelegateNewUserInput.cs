using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Accounts;

public class DelegateNewUserInput
{
    public Guid TargetUserId { get; set; }

    [Required]
    public DateTime? StartTime { get; set; }

    [Required]
    public DateTime? EndTime { get; set; }
}
