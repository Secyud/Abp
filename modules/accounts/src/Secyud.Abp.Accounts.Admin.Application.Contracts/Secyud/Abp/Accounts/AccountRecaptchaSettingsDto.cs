using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Accounts;

public class AccountRecaptchaSettingsDto
{
    public bool UseCaptchaOnLogin { get; set; }

    public bool UseCaptchaOnRegistration { get; set; }

    public string? VerifyBaseUrl { get; set; }

    public string? SiteKey { get; set; }

    public string? SiteSecret { get; set; }

    [Range(2, 3)]
    public int Version { get; set; }

    [Range(0.1, 1.0)]
    public double Score { get; set; }

}
