namespace Secyud.Abp.Accounts.Security.Recaptcha;

public interface IRecaptchaValidator
{
    Task ValidateAsync(string? captchaResponse);
}
