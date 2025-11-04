using System.Security.Claims;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Identities;

/// <summary>
/// Represents a role in the identity system
/// </summary>
public class IdentityRole : AggregateRoot<Guid>, IMultiTenant, IHasEntityVersion, IHasCreationTime
{
    public Guid? TenantId { get; protected set; }

    /// <summary>
    /// Gets or sets the name for this role.
    /// </summary>
    public string Name { get; protected internal set; } = string.Empty;

    /// <summary>
    /// Gets or sets the normalized name for this role.
    /// </summary>
    [DisableAuditing]
    public string NormalizedName { get; protected internal set; } = string.Empty;

    /// <summary>
    /// A default role is automatically assigned to a new user
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// A static role can not be deleted/renamed
    /// </summary>
    public bool IsStatic { get; set; }

    /// <summary>
    /// A user can see other user's public roles
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// A version value that is increased whenever the entity is changed.
    /// </summary>
    public int EntityVersion { get; protected set; }

    public DateTime CreationTime { get; protected set; }

    /// <summary>
    /// Navigation property for claims in this role.
    /// </summary>
    public virtual List<IdentityRoleClaim> Claims { get; protected set; } = null!;

    /// <summary>
    /// Initializes a new instance of <see cref="IdentityRole"/>.
    /// </summary>
    protected IdentityRole()
    {
    }

    public IdentityRole(Guid id, string name, Guid? tenantId = null)
        : base(id)
    {
        Check.NotNull(name, nameof(name));

        Name = name;
        TenantId = tenantId;
        NormalizedName = name.ToUpperInvariant();
        Claims = [];
    }

    public virtual void AddClaim(IGuidGenerator guidGenerator, Claim claim)
    {
        Check.NotNull(guidGenerator, nameof(guidGenerator));
        Check.NotNull(claim, nameof(claim));

        Claims.Add(new IdentityRoleClaim(guidGenerator.Create(), Id, claim, TenantId));
    }

    public virtual void AddClaims(IGuidGenerator guidGenerator, List<Claim> claims)
    {
        Check.NotNull(guidGenerator, nameof(guidGenerator));
        Check.NotNull(claims, nameof(claims));

        foreach (var claim in claims)
        {
            AddClaim(guidGenerator, claim);
        }
    }

    public virtual IdentityRoleClaim? FindClaim(Claim claim)
    {
        Check.NotNull(claim, nameof(claim));

        return Claims.FirstOrDefault(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
    }

    public virtual void RemoveClaim(Claim claim)
    {
        Check.NotNull(claim, nameof(claim));

        Claims.RemoveAll(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
    }

    public virtual void ChangeName(string? name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var oldName = Name;
        Name = name;

        AddDistributedEvent(
            new IdentityRoleNameChangedEto
            {
                Id = Id,
                Name = Name,
                OldName = oldName,
                TenantId = TenantId
            }
        );
    }

    public override string ToString()
    {
        return $"{base.ToString()}, Name = {Name}";
    }
}