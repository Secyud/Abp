using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Identities.Users;

public class UserLockViewModel
{
    public Guid Id { get; set; }

    [Required]
    public DateTime LockoutEnd { get; set; }
}