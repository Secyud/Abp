using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Identities;

public class IdentityOAuthSettingsDto
{
    [Display(Name = "DisplayName:Abp.Identity.EnableOAuthLogin")]
    public bool EnableOAuthLogin { get; set; }

    [Required]
    [Display(Name = "DisplayName:Abp.Identity.ClientId")]
    public string? ClientId { get; set; }

    [Display(Name = "DisplayName:Abp.Identity.ClientSecret")]
    public string? ClientSecret { get; set; }

    [Required]
    [Display(Name = "DisplayName:Abp.Identity.Authority")]
    public string? Authority { get; set; }

    [Display(Name = "DisplayName:Abp.Identity.Scope")]
    public string? Scope { get; set; }

    [Display(Name = "DisplayName:Abp.Identity.RequireHttpsMetadata")]
    public bool RequireHttpsMetadata { get; set; }

    [Display(Name = "DisplayName:Abp.Identity.ValidateEndpoints")]
    public bool ValidateEndpoints { get; set; }

    [Display(Name = "DisplayName:Abp.Identity.ValidateIssuerName")]
    public bool ValidateIssuerName { get; set; }
}