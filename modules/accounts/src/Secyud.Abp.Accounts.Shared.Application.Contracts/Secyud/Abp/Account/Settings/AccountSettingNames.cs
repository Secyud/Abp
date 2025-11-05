namespace Secyud.Abp.Account.Settings;

public static class AccountSettingNames
{
    public const string IsSelfRegistrationEnabled = "Abp.Account.IsSelfRegistrationEnabled";

    public const string EnableLocalLogin = "Abp.Account.EnableLocalLogin";

    public const string PreventEmailEnumeration = "Abp.Account.PreventEmailEnumeration";

    public const string ProfilePictureSource = "Abp.Account.ProfilePictureSource";

    public static class TwoFactorLogin
    {
        public const string IsRememberBrowserEnabled = "Abp.Account.TwoFactorLogin.IsRememberBrowserEnabled";
    }

    public class Captcha
    {
        public const string UseCaptchaOnRegistration = "Abp.Account.Captcha.UseCaptchaOnRegistration";

        public const string UseCaptchaOnLogin = "Abp.Account.Captcha.UseCaptchaOnLogin";

        public const string VerifyBaseUrl = "Abp.Account.Captcha.VerifyBaseUrl";

        public const string SiteKey = "Abp.Account.Captcha.SiteKey";

        public const string SiteSecret = "Abp.Account.Captcha.SiteSecret";

        public const string Version = "Abp.Account.Captcha.Version";

        public const string Score = "Abp.Account.Captcha.Score";
    }

    public const string ExternalProviders = "Abp.Account.ExternalProviders";

    public const string VerifyPasswordDuringExternalLogin = "Abp.Account.VerifyPasswordDuringExternalLogin";

    public const string ExternalLoginPasswordVerified = "Abp.Account.ExternalLoginPasswordVerified";
}
