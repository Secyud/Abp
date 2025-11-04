using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Secyud.Abp.Identities;

public class CreateClaimTypeDto() : ExtensibleObject(false)
{
    [Required]
    public string Name { get; set; } = "";

    public bool Required { get; set; }

    public string? Regex { get; set; }

    public string? RegexDescription { get; set; }

    public string? Description { get; set; }

    public IdentityClaimValueType ValueType { get; set; }
}
