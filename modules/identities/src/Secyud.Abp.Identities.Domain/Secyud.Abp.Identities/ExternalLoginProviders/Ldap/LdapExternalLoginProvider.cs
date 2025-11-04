using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Secyud.Abp.Identities.Features;
using Secyud.Abp.Identities.Settings;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Guids;
using Volo.Abp.Ldap;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace Secyud.Abp.Identities.ExternalLoginProviders.Ldap;

public class LdapExternalLoginProvider(
    IGuidGenerator guidGenerator,
    ICurrentTenant currentTenant,
    IdentityUserManager userManager,
    IIdentityUserRepository identityUserRepository,
    OpenLdapManager ldapManager,
    ILdapSettingProvider ldapSettingProvider,
    IFeatureChecker featureChecker,
    ISettingProvider settingProvider,
    IOptions<IdentityOptions> identityOptions)
    : ExternalLoginProviderBase(guidGenerator, currentTenant, userManager, identityUserRepository, identityOptions), ITransientDependency
{
    public const string Name = "Ldap";

    public ILogger<LdapExternalLoginProvider> Logger { get; set; } = NullLogger<LdapExternalLoginProvider>.Instance;

    protected OpenLdapManager LdapManager { get; } = ldapManager;

    protected ILdapSettingProvider LdapSettingProvider { get; } = ldapSettingProvider;

    protected IFeatureChecker FeatureChecker { get; } = featureChecker;

    protected ISettingProvider SettingProvider { get; } = settingProvider;

    public override async Task<bool> TryAuthenticateAsync(string userName, string? plainPassword)
    {
        Logger.LogInformation("Try to use LDAP for external authentication");
        Check.NotNull(plainPassword, nameof(plainPassword));

        if (!await IsEnabledAsync())
        {
            return false;
        }

        return await LdapManager.AuthenticateAsync(await NormalizeUserNameAsync(userName), plainPassword);
    }

    public override async Task<bool> IsEnabledAsync()
    {
        if (!await FeatureChecker.IsEnabledAsync(IdentitiesFeature.EnableLdapLogin))
        {
            Logger.LogWarning("Ldap login feature is not enabled!");
            return false;
        }

        if (!await SettingProvider.IsTrueAsync(IdentitiesSettingNames.EnableLdapLogin))
        {
            Logger.LogWarning("Ldap login setting is not enabled!");
            return false;
        }

        return true;
    }

    protected override async Task<ExternalLoginUserInfo> GetUserInfoAsync(string? userName)
    {
        var email = await LdapManager.GetUserEmailAsync(userName);
        if (email.IsNullOrWhiteSpace())
        {
            throw new Exception("Unable to get the email of ldap user!");
        }

        return new ExternalLoginUserInfo(email);
    }

    protected virtual async Task<string> NormalizeUserNameAsync(string userName)
    {
        return $"uid={userName}, {await LdapSettingProvider.GetBaseDcAsync()}";
    }
}