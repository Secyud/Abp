using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Identities;

public class IdentityPasswordSettingsDto
{
    [Range(2, 128)]
    [Display(Name = "Abp.Identity.Password.RequiredLength")]
    public int RequiredLength { get; set; }

    [Range(1, 128)]
    [Display(Name = "Abp.Identity.Password.RequiredUniqueChars")]
    public int RequiredUniqueChars { get; set; }

    public bool RequireNonAlphanumeric { get; set; }

    public bool RequireLowercase { get; set; }

    public bool RequireUppercase { get; set; }

    public bool RequireDigit { get; set; }

    public bool ForceUsersToPeriodicallyChangePassword { get; set; }

    public int PasswordChangePeriodDays { get; set; }
}