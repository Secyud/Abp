namespace Secyud.Abp.Accounts.Security.Recaptcha;

public interface IAbpRecaptchaValidatorFactory
{
    Task<IRecaptchaValidator> CreateAsync();
}
