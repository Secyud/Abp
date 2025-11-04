using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Identities;

public class IdentityUserUpdateRolesDto
{
    [Required]
    public string[] RoleNames { get; set; } = [];
}
