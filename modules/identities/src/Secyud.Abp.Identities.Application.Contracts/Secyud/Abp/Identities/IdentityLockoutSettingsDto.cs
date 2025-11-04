using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Identities;

public class IdentityLockoutSettingsDto
{
    public bool AllowedForNewUsers { get; set; }

    [Display(Name = "Abp.Identity.Lockout.LockoutDuration")]
    public int LockoutDuration { get; set; }

    [Display(Name = "Abp.Identity.Lockout.MaxFailedAccessAttempts")]
    public int MaxFailedAccessAttempts { get; set; }
}