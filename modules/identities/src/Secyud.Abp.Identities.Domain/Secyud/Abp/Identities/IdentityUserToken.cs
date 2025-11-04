using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Identities;

/// <summary>
/// Represents an authentication token for a user.
/// </summary>
public class IdentityUserToken : Entity, IMultiTenant
{
    public Guid? TenantId { get; protected set; }

    /// <summary>
    /// Gets or sets the primary key of the user that the token belongs to.
    /// </summary>
    public Guid UserId { get; protected set; }

    /// <summary>
    /// Gets or sets the LoginProvider this token is from.
    /// </summary>
    public string LoginProvider { get; protected set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the token.
    /// </summary>
    public string Name { get; protected set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token value.
    /// </summary>
    public string? Value { get; set; }

    protected IdentityUserToken()
    {
    }

    protected internal IdentityUserToken(
        Guid userId,
        string loginProvider,
        string name,
        string? value,
        Guid? tenantId)
    {
        Check.NotNull(loginProvider, nameof(loginProvider));
        Check.NotNull(name, nameof(name));

        UserId = userId;
        LoginProvider = loginProvider;
        Name = name;
        Value = value;
        TenantId = tenantId;
    }

    public override object[] GetKeys()
    {
        return [UserId, LoginProvider, Name];
    }
}