using System.ComponentModel.DataAnnotations;
using Secyud.Abp.Identities.Settings;

namespace Secyud.Abp.Identities;

public class IdentitySessionSettingsDto
{
    [Display(Name = "DisplayName:Abp.Identity.PreventConcurrentLogin")]
    public IdentitiesPreventConcurrentLoginBehaviour PreventConcurrentLogin { get; set; }
}
