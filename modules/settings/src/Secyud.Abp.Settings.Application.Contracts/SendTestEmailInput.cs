using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Settings;

public class SendTestEmailInput
{
    [Required]
    public string SenderEmailAddress { get; set; } = string.Empty;

    [Required]
    public string TargetEmailAddress { get; set; } = string.Empty;

    [Required]
    public string Subject { get; set; } = string.Empty;

    public string? Body { get; set; }
}