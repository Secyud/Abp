using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace Secyud.Abp.Tenants;

public class TenantCreateDto : TenantCreateOrUpdateDtoBase
{
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string AdminEmailAddress { get; set; } = "";

    [Required]
    [MaxLength(128)]
    [DisableAuditing]
    public string AdminPassword { get; set; } = "";
}