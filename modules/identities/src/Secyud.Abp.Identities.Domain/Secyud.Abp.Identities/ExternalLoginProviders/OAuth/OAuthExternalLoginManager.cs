using System.Security.Claims;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Secyud.Abp.Identities.Settings;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Identities.ExternalLoginProviders.OAuth;

public class OAuthExternalLoginManager(
    IOAuthSettingProvider oAuthSettingProvider,
    IHttpClientFactory httpClientFactory)
    : ITransientDependency
{
    public const string HttpClientName = "OAuthExternalLoginManager";

    public ILogger<OAuthExternalLoginManager> Logger { get; set; } = NullLogger<OAuthExternalLoginManager>.Instance;

    protected IOAuthSettingProvider OAuthSettingProvider { get; } = oAuthSettingProvider;
    protected IHttpClientFactory HttpClientFactory { get; } = httpClientFactory;

    public virtual async Task<bool> AuthenticateAsync(string userName, string? password)
    {
        try
        {
            await GetAccessTokenAsync(userName, password);
            return true;
        }
        catch (AbpException ex)
        {
            Logger.LogException(ex);
            return false;
        }
    }

    public virtual async Task<IEnumerable<Claim>> GetUserInfoAsync(string userName, string? password)
    {
        using var httpClient = HttpClientFactory.CreateClient(HttpClientName);
        var token = await GetAccessTokenAsync(userName, password);
        var discoveryResponse = await GetDiscoveryResponseAsync();

        var userinfoResponse = await httpClient.GetUserInfoAsync(
            new UserInfoRequest
            {
                Token = token,
                Address = discoveryResponse.UserInfoEndpoint
            });

        if (userinfoResponse.IsError)
        {
            throw userinfoResponse.Exception ?? new AbpException("Get user info error: " + userinfoResponse.Raw);
        }

        return userinfoResponse.Claims;
    }

    protected virtual async Task<string?> GetAccessTokenAsync(string userName, string? password)
    {

        using var httpClient = HttpClientFactory.CreateClient(HttpClientName);
        var discoveryResponse = await GetDiscoveryResponseAsync();

        var request = new PasswordTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = await OAuthSettingProvider.GetClientIdAsync() ?? "",
            ClientSecret = await OAuthSettingProvider.GetClientSecretAsync(),
            Scope = await OAuthSettingProvider.GetScopeAsync(),
            UserName = userName,
            Password = password
        };

        var tokenResponse = await httpClient.RequestPasswordTokenAsync(request);
        if (tokenResponse.IsError)
        {
            throw tokenResponse.Exception ?? new AbpException("Get access token error: " + tokenResponse.Raw);
        }

        return tokenResponse.AccessToken;
    }

    protected virtual async Task<DiscoveryDocumentResponse> GetDiscoveryResponseAsync()
    {
        using var httpClient = HttpClientFactory.CreateClient(HttpClientName);
        var request = new DiscoveryDocumentRequest
        {
            Address = await OAuthSettingProvider.GetAuthorityAsync(),
            Policy = new DiscoveryPolicy
            {
                RequireHttps = await OAuthSettingProvider.GetRequireHttpsMetadataAsync(),
                ValidateIssuerName = await OAuthSettingProvider.GetValidateIssuerNameAsync(),
                ValidateEndpoints = await OAuthSettingProvider.GetValidateEndpointsAsync()
            }
        };

        var discoveryResponse = await httpClient.GetDiscoveryDocumentAsync(request);
        if (discoveryResponse.IsError)
        {
            throw discoveryResponse.Exception ?? new AbpException("Get discovery error: " + discoveryResponse.Raw);
        }

        return discoveryResponse;
    }
}