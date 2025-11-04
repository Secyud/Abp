using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace Secyud.Abp.Identities;

public class IdentityUserUpdatePasswordInput
{
    [Required]
    [DisableAuditing]
    public string NewPassword { get; set; }= "";
}
