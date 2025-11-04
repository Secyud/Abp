using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace Secyud.Abp.Identities;

public class ImportExternalUserInput
{
    [Required]
    public string Provider { get; set; }= "";

    [Required]
    public string UserNameOrEmailAddress { get; set; }= "";

    [DisableAuditing]
    public string? Password { get; set; }
}