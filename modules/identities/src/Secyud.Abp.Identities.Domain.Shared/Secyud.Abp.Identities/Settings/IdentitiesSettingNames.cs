namespace Secyud.Abp.Identities.Settings;

public static class IdentitiesSettingNames
{
    private const string Prefix = "Abp.Identity";

    public static class Password
    {
        private const string PasswordPrefix = Prefix + ".Password";

        public const string RequiredLength = PasswordPrefix + ".RequiredLength";
        public const string RequiredUniqueChars = PasswordPrefix + ".RequiredUniqueChars";
        public const string RequireNonAlphanumeric = PasswordPrefix + ".RequireNonAlphanumeric";
        public const string RequireLowercase = PasswordPrefix + ".RequireLowercase";
        public const string RequireUppercase = PasswordPrefix + ".RequireUppercase";
        public const string RequireDigit = PasswordPrefix + ".RequireDigit";
        public const string ForceUsersToPeriodicallyChangePassword = PasswordPrefix + ".ForceUsersToPeriodicallyChangePassword";
        public const string PasswordChangePeriodDays = PasswordPrefix + ".PasswordChangePeriodDays";
    }

    public static class Lockout
    {
        private const string LockoutPrefix = Prefix + ".Lockout";

        public const string AllowedForNewUsers = LockoutPrefix + ".AllowedForNewUsers";
        public const string LockoutDuration = LockoutPrefix + ".LockoutDuration";
        public const string MaxFailedAccessAttempts = LockoutPrefix + ".MaxFailedAccessAttempts";
    }

    public static class SignIn
    {
        private const string SignInPrefix = Prefix + ".SignIn";

        public const string RequireConfirmedEmail = SignInPrefix + ".RequireConfirmedEmail";
        public const string RequireEmailVerificationToRegister = SignInPrefix + ".RequireEmailVerificationToRegister";
        public const string EnablePhoneNumberConfirmation = SignInPrefix + ".EnablePhoneNumberConfirmation";
        public const string RequireConfirmedPhoneNumber = SignInPrefix + ".RequireConfirmedPhoneNumber";
    }

    public static class User
    {
        private const string UserPrefix = Prefix + ".User";

        public const string IsUserNameUpdateEnabled = UserPrefix + ".IsUserNameUpdateEnabled";
        public const string IsEmailUpdateEnabled = UserPrefix + ".IsEmailUpdateEnabled";
    }

    public const string EnableLdapLogin = "Abp.Account.EnableLdapLogin";

    public const string EnableOAuthLogin = Prefix + ".EnableOAuthLogin";

    public static class TwoFactor
    {
        private const string TwoFactorPrefix = Prefix + ".TwoFactor";

        public const string Behaviour = TwoFactorPrefix + ".Behaviour";

        public const string UsersCanChange = TwoFactorPrefix + ".UsersCanChange";
    }

    public static class OAuthLogin
    {
        private const string OAuthLoginPrefix = Prefix + ".OAuthLogin";

        public const string ClientId = OAuthLoginPrefix + ".ClientId";

        public const string ClientSecret = OAuthLoginPrefix + ".ClientSecret";

        public const string Scope = OAuthLoginPrefix + ".Scope";

        public const string Authority = OAuthLoginPrefix + ".Authority";

        public const string RequireHttpsMetadata = OAuthLoginPrefix + ".RequireHttpsMetadata";

        public const string ValidateEndpoints = OAuthLoginPrefix + ".ValidateEndpoints";

        public const string ValidateIssuerName = OAuthLoginPrefix + ".ValidateIssuerName";
    }

    public static class Session
    {
        private const string SessionPrefix = Prefix + ".Session";

        public const string PreventConcurrentLogin = SessionPrefix + ".PreventConcurrentLogin";
    }
}