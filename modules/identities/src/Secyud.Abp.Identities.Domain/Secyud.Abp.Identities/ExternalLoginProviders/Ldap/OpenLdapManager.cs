using LdapForNet;
using LdapForNet.Native;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ldap;

namespace Secyud.Abp.Identities.ExternalLoginProviders.Ldap;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(OpenLdapManager), typeof(ILdapManager), typeof(LdapManager))]
public class OpenLdapManager(ILdapSettingProvider ldapSettingProvider) : LdapManager(ldapSettingProvider)
{
    public virtual async Task<string?> GetUserEmailAsync(string? userName)
    {
        using var conn = await CreateLdapConnectionAsync();
        var adminName = await NormalizeUserNameAsync(await LdapSettingProvider.GetUserNameAsync());
        var password = await LdapSettingProvider.GetPasswordAsync() ?? "";
        await AuthenticateLdapConnectionAsync(conn, adminName, password);

        var searchResults = await conn.SearchAsync(await GetBaseDnAsync(), await GetUserFilterAsync(userName));
        try
        {
            var userEntry = searchResults.First();
            return await GetUserEmailAsync(userEntry);
        }
        catch (LdapException e)
        {
            Logger.LogException(e);
        }

        return null;
    }

    protected override async Task ConnectAsync(ILdapConnection ldapConnection)
    {
        var schema = await LdapSettingProvider.GetLdapOverSsl() ? Native.LdapSchema.LDAPS : Native.LdapSchema.LDAP;
        ldapConnection.Connect(await LdapSettingProvider.GetServerHostAsync(), await LdapSettingProvider.GetServerPortAsync(), schema);
    }

    protected virtual async Task<string> NormalizeUserNameAsync(string? userName)
    {
        return $"cn={userName},{await LdapSettingProvider.GetBaseDcAsync()}";
    }

    protected virtual Task<string?> GetUserEmailAsync(LdapEntry ldapEntry)
    {
        return Task.FromResult(ldapEntry.ToDirectoryEntry().GetAttribute("mail")?.GetValue<string>());
    }

    protected virtual async Task<string?> GetBaseDnAsync()
    {
        return await LdapSettingProvider.GetBaseDcAsync();
    }

    protected virtual Task<string> GetUserFilterAsync(string? userName)
    {
        return Task.FromResult($"(&(uid={userName}))");
    }
}