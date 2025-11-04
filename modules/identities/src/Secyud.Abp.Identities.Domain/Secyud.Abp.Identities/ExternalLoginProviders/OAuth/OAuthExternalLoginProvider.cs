using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Secyud.Abp.Identities.Features;
using Secyud.Abp.Identities.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace Secyud.Abp.Identities.ExternalLoginProviders.OAuth;

public class OAuthExternalLoginProvider(
    IGuidGenerator guidGenerator,
    ICurrentTenant currentTenant,
    IdentityUserManager userManager,
    IIdentityUserRepository identityUserRepository,
    IOptions<IdentityOptions> identityOptions,
    OAuthExternalLoginManager oAuthExternalLoginManager,
    ISettingProvider settingProvider,
    IFeatureChecker featureChecker,
    IOptions<AbpOAuthExternalLoginProviderOptions> options)
    : ExternalLoginProviderWithPasswordBase(guidGenerator,
        currentTenant,
        userManager,
        identityUserRepository,
        identityOptions,
        options.Value.CanObtainUserInfoWithoutPassword), ITransientDependency
{
    public const string Name = "OAuth";

    public ILogger<OAuthExternalLoginProvider> Logger { get; set; } = NullLogger<OAuthExternalLoginProvider>.Instance;

    protected OAuthExternalLoginManager OAuthExternalLoginManager { get; } = oAuthExternalLoginManager;

    protected ISettingProvider SettingProvider { get; } = settingProvider;

    protected IFeatureChecker FeatureChecker { get; } = featureChecker;

    protected AbpOAuthExternalLoginProviderOptions Options { get; } = options.Value;

    public override async Task<bool> TryAuthenticateAsync(string userName, string? plainPassword)
    {
        Logger.LogInformation("Try to use OAUTH for external authentication");

        if (!await IsEnabledAsync())
        {
            return false;
        }

        return await OAuthExternalLoginManager.AuthenticateAsync(userName, plainPassword);
    }

    public override async Task<bool> IsEnabledAsync()
    {
        if (!await FeatureChecker.IsEnabledAsync(IdentitiesFeature.EnableOAuthLogin))
        {
            Logger.LogWarning("OAuth login feature is not enabled!");
            return false;
        }

        if (!await SettingProvider.IsTrueAsync(IdentitiesSettingNames.EnableOAuthLogin))
        {
            Logger.LogWarning("OAuth login setting is not enabled!");
            return false;
        }

        return true;
    }

    protected override async Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName, string? plainPassword)
    {
        return await MapClaimsToExternalLoginUserInfoAsync(await OAuthExternalLoginManager.GetUserInfoAsync(userName, plainPassword));
    }

    protected virtual Task<ExternalLoginUserInfo> MapClaimsToExternalLoginUserInfoAsync(IEnumerable<Claim> claims)
    {
        var claimsArray = claims as Claim[] ?? claims.ToArray();

        var email = claimsArray.FirstOrDefault(x => x.Type == Options.EmailClaimType);
        if (email == null)
        {
            throw new Exception("Unable to get the email of external user!");
        }

        var userInfo = new ExternalLoginUserInfo(email.Value)
        {
            Name = claimsArray.FirstOrDefault(x => x.Type == Options.NameClaimType)?.Value,
            Surname = claimsArray.FirstOrDefault(x => x.Type == Options.SurnameClaimType)?.Value,
            EmailConfirmed = claimsArray.FirstOrDefault(x => x.Type == Options.EmailConfirmedClaimType)?.Value.To<bool>() ?? false,
            PhoneNumber = claimsArray.FirstOrDefault(x => x.Type == Options.PhoneNumberClaimType)?.Value,
            PhoneNumberConfirmed = claimsArray.FirstOrDefault(x => x.Type == Options.PhoneNumberConfirmedClaimType)?.Value.To<bool>() ?? false,
            ProviderKey = claimsArray.FirstOrDefault(x => x.Type == Options.ProviderKeyClaimType)?.Value ?? ""
        };

        return Task.FromResult(userInfo);
    }
}