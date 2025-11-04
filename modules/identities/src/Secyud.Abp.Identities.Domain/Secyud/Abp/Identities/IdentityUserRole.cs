using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Identities;

/// <summary>
/// Represents the link between a user and a role.
/// </summary>
public class IdentityUserRole : Entity, IMultiTenant
{
    public Guid? TenantId { get; protected set; }

    /// <summary>
    /// Gets or sets the primary key of the user that is linked to a role.
    /// </summary>
    public Guid UserId { get; protected set; }

    /// <summary>
    /// Gets or sets the primary key of the role that is linked to the user.
    /// </summary>
    public Guid RoleId { get; protected set; }

    protected IdentityUserRole()
    {

    }

    protected internal IdentityUserRole(Guid userId, Guid roleId, Guid? tenantId)
    {
        UserId = userId;
        RoleId = roleId;
        TenantId = tenantId;
    }

    public override object[] GetKeys()
    {
        return [UserId, RoleId];
    }
}
