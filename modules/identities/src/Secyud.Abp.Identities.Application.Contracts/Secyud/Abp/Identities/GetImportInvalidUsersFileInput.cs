using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Identities;

public class GetImportInvalidUsersFileInput
{
    [Required]
    public string Token { get; set; } = string.Empty;
}