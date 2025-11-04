using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Secyud.Abp.Identities.Features;
using Secyud.Abp.Identities.Settings;
using Secyud.Abp.Settings;
using Volo.Abp.Features;
using Volo.Abp.Ldap;
using Volo.Abp.Settings;

namespace Secyud.Abp.Identities;

[Authorize(IdentityPermissions.Settings.DefaultName)]
public class IdentitySettingsAppService(ISettingManager settingManager, IOptions<IdentityOptions> identityOptions)
    : IdentityAppServiceBase, IIdentitySettingsAppService
{
    protected ISettingManager SettingManager { get; } = settingManager;
    protected IOptions<IdentityOptions> IdentityOptions { get; } = identityOptions;

    public virtual async Task<IdentitySettingsDto> GetAsync()
    {
        await IdentityOptions.SetAsync();

        return new IdentitySettingsDto
        {
            Password = new IdentityPasswordSettingsDto
            {
                RequiredLength = IdentityOptions.Value.Password.RequiredLength,
                RequiredUniqueChars = IdentityOptions.Value.Password.RequiredUniqueChars,
                RequireNonAlphanumeric = IdentityOptions.Value.Password.RequireNonAlphanumeric,
                RequireLowercase = IdentityOptions.Value.Password.RequireLowercase,
                RequireUppercase = IdentityOptions.Value.Password.RequireUppercase,
                RequireDigit = IdentityOptions.Value.Password.RequireDigit,

                ForceUsersToPeriodicallyChangePassword =
                    await SettingProvider.GetAsync(IdentitiesSettingNames.Password.ForceUsersToPeriodicallyChangePassword, false),
                PasswordChangePeriodDays = await SettingProvider.GetAsync<int>(IdentitiesSettingNames.Password.PasswordChangePeriodDays),
            },
            Lockout = new IdentityLockoutSettingsDto
            {
                AllowedForNewUsers = IdentityOptions.Value.Lockout.AllowedForNewUsers,
                LockoutDuration = (int)IdentityOptions.Value.Lockout.DefaultLockoutTimeSpan.TotalSeconds,
                MaxFailedAccessAttempts = IdentityOptions.Value.Lockout.MaxFailedAccessAttempts
            },
            SignIn = new IdentitySignInSettingsDto
            {
                RequireConfirmedEmail = IdentityOptions.Value.SignIn.RequireConfirmedEmail,
                EnablePhoneNumberConfirmation = await SettingProvider.GetAsync(IdentitiesSettingNames.SignIn.EnablePhoneNumberConfirmation, true),
                RequireConfirmedPhoneNumber = IdentityOptions.Value.SignIn.RequireConfirmedPhoneNumber
            },
            User = new IdentityUserSettingsDto
            {
                IsEmailUpdateEnabled = await SettingProvider.GetAsync(IdentitiesSettingNames.User.IsEmailUpdateEnabled, true),
                IsUserNameUpdateEnabled = await SettingProvider.GetAsync(IdentitiesSettingNames.User.IsUserNameUpdateEnabled, true)
            }
        };
    }

    public virtual async Task UpdateAsync(IdentitySettingsDto input)
    {
        await IdentityOptions.SetAsync();

        if (input.Password != null)
        {
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.Password.RequiredLength, input.Password.RequiredLength.ToString());
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.Password.RequiredUniqueChars, input.Password.RequiredUniqueChars.ToString());
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.Password.RequireNonAlphanumeric,
                input.Password.RequireNonAlphanumeric.ToString());
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.Password.RequireLowercase, input.Password.RequireLowercase.ToString());
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.Password.RequireUppercase, input.Password.RequireUppercase.ToString());
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.Password.RequireDigit, input.Password.RequireDigit.ToString());

            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.Password.ForceUsersToPeriodicallyChangePassword,
                input.Password.ForceUsersToPeriodicallyChangePassword.ToString());
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.Password.PasswordChangePeriodDays,
                input.Password.PasswordChangePeriodDays.ToString());
        }

        if (input.Lockout != null)
        {
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.Lockout.AllowedForNewUsers, input.Lockout.AllowedForNewUsers.ToString());
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.Lockout.MaxFailedAccessAttempts,
                input.Lockout.MaxFailedAccessAttempts.ToString());
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.Lockout.LockoutDuration, input.Lockout.LockoutDuration.ToString());
        }

        if (input.SignIn != null)
        {
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.SignIn.RequireConfirmedEmail, input.SignIn.RequireConfirmedEmail.ToString());
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.SignIn.RequireConfirmedPhoneNumber,
                input.SignIn.RequireConfirmedPhoneNumber.ToString());

            var enablePhoneNumberConfirmationValue = input.SignIn.EnablePhoneNumberConfirmation || input.SignIn.RequireConfirmedPhoneNumber;
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.SignIn.EnablePhoneNumberConfirmation,
                enablePhoneNumberConfirmationValue.ToString());
        }

        if (input.User != null)
        {
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.User.IsUserNameUpdateEnabled, input.User.IsUserNameUpdateEnabled.ToString());
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.User.IsEmailUpdateEnabled, input.User.IsEmailUpdateEnabled.ToString());
        }
    }


    [RequiresFeature(IdentitiesFeature.EnableLdapLogin)]
    public virtual async Task<IdentityLdapSettingsDto> GetLdapAsync()
    {
        return new IdentityLdapSettingsDto
        {
            EnableLdapLogin = await SettingProvider.GetAsync<bool>(IdentitiesSettingNames.EnableLdapLogin),
            Ldaps = await SettingProvider.GetAsync<bool>(LdapSettingNames.Ldaps),
            LdapServerHost = await SettingProvider.GetOrNullAsync(LdapSettingNames.ServerHost),
            LdapServerPort = await SettingProvider.GetOrNullAsync(LdapSettingNames.ServerPort),
            LdapBaseDc = await SettingProvider.GetOrNullAsync(LdapSettingNames.BaseDc),
            LdapDomain = await SettingProvider.GetOrNullAsync(LdapSettingNames.Domain),
            LdapUserName = await SettingProvider.GetOrNullAsync(LdapSettingNames.UserName),
            //LdapPassword = await SettingProvider.GetOrNullAsync(LdapSettingNames.Password)
        };
    }

    [RequiresFeature(IdentitiesFeature.EnableLdapLogin)]
    public virtual async Task UpdateLdapAsync(IdentityLdapSettingsDto? input)
    {
        if (input != null)
        {
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.EnableLdapLogin, input.EnableLdapLogin.ToString());
            await SettingManager.SetForCurrentTenantAsync(LdapSettingNames.Ldaps, input.Ldaps.ToString());
            await SettingManager.SetForCurrentTenantAsync(LdapSettingNames.ServerHost, input.LdapServerHost);
            await SettingManager.SetForCurrentTenantAsync(LdapSettingNames.ServerPort, input.LdapServerPort);
            await SettingManager.SetForCurrentTenantAsync(LdapSettingNames.BaseDc, input.LdapBaseDc);
            await SettingManager.SetForCurrentTenantAsync(LdapSettingNames.Domain, input.LdapDomain);
            await SettingManager.SetForCurrentTenantAsync(LdapSettingNames.UserName, input.LdapUserName);
            if (!input.LdapPassword.IsNullOrWhiteSpace())
            {
                await SettingManager.SetForCurrentTenantAsync(LdapSettingNames.Password, input.LdapPassword);
            }
        }
    }

    [RequiresFeature(IdentitiesFeature.EnableOAuthLogin)]
    public virtual async Task<IdentityOAuthSettingsDto> GetOAuthAsync()
    {
        return new IdentityOAuthSettingsDto
        {
            EnableOAuthLogin = await SettingProvider.GetAsync<bool>(IdentitiesSettingNames.EnableOAuthLogin),
            ClientId = await SettingProvider.GetOrNullAsync(IdentitiesSettingNames.OAuthLogin.ClientId),
            ClientSecret = await SettingProvider.GetOrNullAsync(IdentitiesSettingNames.OAuthLogin.ClientSecret),
            RequireHttpsMetadata = await SettingProvider.GetAsync<bool>(IdentitiesSettingNames.OAuthLogin.RequireHttpsMetadata),
            Scope = await SettingProvider.GetOrNullAsync(IdentitiesSettingNames.OAuthLogin.Scope),
            Authority = await SettingProvider.GetOrNullAsync(IdentitiesSettingNames.OAuthLogin.Authority),
            ValidateEndpoints = await SettingProvider.GetAsync<bool>(IdentitiesSettingNames.OAuthLogin.ValidateEndpoints),
            ValidateIssuerName = await SettingProvider.GetAsync<bool>(IdentitiesSettingNames.OAuthLogin.ValidateIssuerName),
        };
    }

    [RequiresFeature(IdentitiesFeature.EnableOAuthLogin)]
    public virtual async Task UpdateOAuthAsync(IdentityOAuthSettingsDto? input)
    {
        if (input != null)
        {
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.EnableOAuthLogin, input.EnableOAuthLogin.ToString());
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.OAuthLogin.ClientId, input.ClientId);
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.OAuthLogin.ClientSecret, input.ClientSecret);
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.OAuthLogin.RequireHttpsMetadata, input.RequireHttpsMetadata.ToString());
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.OAuthLogin.Scope, input.Scope);
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.OAuthLogin.Authority, input.Authority);
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.OAuthLogin.ValidateEndpoints, input.ValidateEndpoints.ToString());
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.OAuthLogin.ValidateIssuerName, input.ValidateIssuerName.ToString());
        }
    }

    public virtual async Task<IdentitySessionSettingsDto> GetSessionAsync()
    {
        return new IdentitySessionSettingsDto
        {
            PreventConcurrentLogin = await IdentitiesPreventConcurrentLoginBehaviourSettingHelper.Get(SettingProvider)
        };
    }

    public virtual async Task UpdateSessionAsync(IdentitySessionSettingsDto? input)
    {
        if (input != null)
        {
            await SettingManager.SetForCurrentTenantAsync(IdentitiesSettingNames.Session.PreventConcurrentLogin, input.PreventConcurrentLogin.ToString());
        }
    }
}