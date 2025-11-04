using Secyud.Abp.Identities.Localization;
using Secyud.Abp.Identities.Settings;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Secyud.Abp.Identities;

public class AbpIdentitySettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                IdentitiesSettingNames.Password.RequiredLength,
                6.ToString(),
                L("DisplayName:Abp.Identity.Password.RequiredLength"),
                L("Description:Abp.Identity.Password.RequiredLength"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.Password.RequiredUniqueChars,
                1.ToString(),
                L("DisplayName:Abp.Identity.Password.RequiredUniqueChars"),
                L("Description:Abp.Identity.Password.RequiredUniqueChars"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.Password.RequireNonAlphanumeric,
                true.ToString(),
                L("DisplayName:Abp.Identity.Password.RequireNonAlphanumeric"),
                L("Description:Abp.Identity.Password.RequireNonAlphanumeric"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.Password.RequireLowercase,
                true.ToString(),
                L("DisplayName:Abp.Identity.Password.RequireLowercase"),
                L("Description:Abp.Identity.Password.RequireLowercase"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.Password.RequireUppercase,
                true.ToString(),
                L("DisplayName:Abp.Identity.Password.RequireUppercase"),
                L("Description:Abp.Identity.Password.RequireUppercase"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.Password.RequireDigit,
                true.ToString(),
                L("DisplayName:Abp.Identity.Password.RequireDigit"),
                L("Description:Abp.Identity.Password.RequireDigit"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.Password.ForceUsersToPeriodicallyChangePassword,
                false.ToString(),
                L("DisplayName:Abp.Identity.Password.ForceUsersToPeriodicallyChangePassword"),
                L("Description:Abp.Identity.Password.ForceUsersToPeriodicallyChangePassword"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.Password.PasswordChangePeriodDays,
                0.ToString(),
                L("DisplayName:Abp.Identity.Password.PasswordChangePeriodDays"),
                L("Description:Abp.Identity.Password.PasswordChangePeriodDays"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.Lockout.AllowedForNewUsers,
                true.ToString(),
                L("DisplayName:Abp.Identity.Lockout.AllowedForNewUsers"),
                L("Description:Abp.Identity.Lockout.AllowedForNewUsers"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.Lockout.LockoutDuration,
                (5 * 60).ToString(),
                L("DisplayName:Abp.Identity.Lockout.LockoutDuration"),
                L("Description:Abp.Identity.Lockout.LockoutDuration"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.Lockout.MaxFailedAccessAttempts,
                5.ToString(),
                L("DisplayName:Abp.Identity.Lockout.MaxFailedAccessAttempts"),
                L("Description:Abp.Identity.Lockout.MaxFailedAccessAttempts"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.SignIn.RequireConfirmedEmail,
                false.ToString(),
                L("DisplayName:Abp.Identity.SignIn.RequireConfirmedEmail"),
                L("Description:Abp.Identity.SignIn.RequireConfirmedEmail"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.SignIn.EnablePhoneNumberConfirmation,
                true.ToString(),
                L("DisplayName:Abp.Identity.SignIn.EnablePhoneNumberConfirmation"),
                L("Description:Abp.Identity.SignIn.EnablePhoneNumberConfirmation"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.SignIn.RequireEmailVerificationToRegister,
                false.ToString(),
                L("DisplayName:Abp.Identity.SignIn.RequireEmailVerificationToRegister"),
                L("Description:Abp.Identity.SignIn.RequireEmailVerificationToRegister"),
                false),
            new SettingDefinition(
                IdentitiesSettingNames.SignIn.RequireConfirmedPhoneNumber,
                false.ToString(),
                L("DisplayName:Abp.Identity.SignIn.RequireConfirmedPhoneNumber"),
                L("Description:Abp.Identity.SignIn.RequireConfirmedPhoneNumber"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.User.IsUserNameUpdateEnabled,
                true.ToString(),
                L("DisplayName:Abp.Identity.User.IsUserNameUpdateEnabled"),
                L("Description:Abp.Identity.User.IsUserNameUpdateEnabled"),
                true),
            new SettingDefinition(
                IdentitiesSettingNames.User.IsEmailUpdateEnabled,
                true.ToString(),
                L("DisplayName:Abp.Identity.User.IsEmailUpdateEnabled"),
                L("Description:Abp.Identity.User.IsEmailUpdateEnabled"),
                true)
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpIdentitiesResource>(name);
    }
}