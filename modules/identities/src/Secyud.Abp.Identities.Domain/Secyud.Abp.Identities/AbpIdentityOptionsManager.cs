using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Secyud.Abp.Identities.Settings;
using Volo.Abp.Options;
using Volo.Abp.Settings;

namespace Secyud.Abp.Identities;

public class AbpIdentityOptionsManager(
    IOptionsFactory<IdentityOptions> factory,
    ISettingProvider settingProvider
) : AbpDynamicOptionsManager<IdentityOptions>(factory)
{
    protected ISettingProvider SettingProvider { get; } = settingProvider;

    protected override async Task OverrideOptionsAsync(string name, IdentityOptions options)
    {
        options.Password.RequiredLength = await SettingProvider.GetAsync(
            IdentitiesSettingNames.Password.RequiredLength,
            options.Password.RequiredLength);
        options.Password.RequiredUniqueChars = await SettingProvider.GetAsync(
            IdentitiesSettingNames.Password.RequiredUniqueChars,
            options.Password.RequiredUniqueChars);
        options.Password.RequireNonAlphanumeric = await SettingProvider.GetAsync(
            IdentitiesSettingNames.Password.RequireNonAlphanumeric,
            options.Password.RequireNonAlphanumeric);
        options.Password.RequireLowercase = await SettingProvider.GetAsync(
            IdentitiesSettingNames.Password.RequireLowercase,
            options.Password.RequireLowercase);
        options.Password.RequireUppercase = await SettingProvider.GetAsync(
            IdentitiesSettingNames.Password.RequireUppercase,
            options.Password.RequireUppercase);
        options.Password.RequireDigit = await SettingProvider.GetAsync(
            IdentitiesSettingNames.Password.RequireDigit,
            options.Password.RequireDigit);
        options.Lockout.AllowedForNewUsers = await SettingProvider.GetAsync(
            IdentitiesSettingNames.Lockout.AllowedForNewUsers,
            options.Lockout.AllowedForNewUsers);
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(await SettingProvider.GetAsync(
            IdentitiesSettingNames.Lockout.LockoutDuration,
            options.Lockout.DefaultLockoutTimeSpan.TotalSeconds.To<int>()));
        options.Lockout.MaxFailedAccessAttempts = await SettingProvider.GetAsync(
            IdentitiesSettingNames.Lockout.MaxFailedAccessAttempts,
            options.Lockout.MaxFailedAccessAttempts);
        options.SignIn.RequireConfirmedEmail = await SettingProvider.GetAsync(
            IdentitiesSettingNames.SignIn.RequireConfirmedEmail,
            options.SignIn.RequireConfirmedEmail);
        options.SignIn.RequireConfirmedPhoneNumber = await SettingProvider.GetAsync(
            IdentitiesSettingNames.SignIn.RequireConfirmedPhoneNumber,
            options.SignIn.RequireConfirmedPhoneNumber);
    }
}