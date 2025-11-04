using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Identities;

/// <summary>
/// Represents a login and its associated provider for a user.
/// </summary>
public class IdentityUserLogin : Entity, IMultiTenant
{
    public Guid? TenantId { get; protected set; }

    /// <summary>
    /// Gets or sets the of the primary key of the user associated with this login.
    /// </summary>
    public Guid UserId { get; protected set; }

    /// <summary>
    /// Gets or sets the login provider for the login (e.g. facebook, google)
    /// </summary>
    public string LoginProvider { get; protected set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique provider identifier for this login.
    /// </summary>
    public string ProviderKey { get; protected set; } = string.Empty;

    /// <summary>
    /// Gets or sets the friendly name used in a UI for this login.
    /// </summary>
    public string? ProviderDisplayName { get; protected set; }

    protected IdentityUserLogin()
    {
    }

    protected internal IdentityUserLogin(
        Guid userId,
        string loginProvider,
        string providerKey,
        string? providerDisplayName,
        Guid? tenantId)
    {
        Check.NotNull(loginProvider, nameof(loginProvider));
        Check.NotNull(providerKey, nameof(providerKey));

        UserId = userId;
        LoginProvider = loginProvider;
        ProviderKey = providerKey;
        ProviderDisplayName = providerDisplayName;
        TenantId = tenantId;
    }

    protected internal IdentityUserLogin(
        Guid userId, UserLoginInfo login, Guid? tenantId)
        : this(userId, login.LoginProvider, login.ProviderKey, login.ProviderDisplayName, tenantId)
    {
    }

    public virtual UserLoginInfo ToUserLoginInfo()
    {
        return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
    }

    public override object[] GetKeys()
    {
        return [UserId, LoginProvider];
    }
}