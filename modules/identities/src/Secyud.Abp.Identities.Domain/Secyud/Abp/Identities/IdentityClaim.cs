using System.Security.Claims;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Identities;

public abstract class IdentityClaim : Entity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; protected set; }

    /// <summary>
    /// Gets or sets the claim type for this claim.
    /// </summary>
    public string ClaimType { get; protected set; } = "";

    /// <summary>
    /// Gets or sets the claim value for this claim.
    /// </summary>
    public string ClaimValue { get; protected set; } = "";

    protected IdentityClaim()
    {
    }

    protected internal IdentityClaim(Guid id, Claim claim, Guid? tenantId) : this(id, claim.Type, claim.Value, tenantId)
    {
    }

    protected internal IdentityClaim(Guid id, string claimType, string claimValue, Guid? tenantId)
        : base(id)
    {
        Check.NotNull(claimType, nameof(claimType));

        ClaimType = claimType;
        ClaimValue = claimValue;
        TenantId = tenantId;
    }

    /// <summary>
    /// Creates a Claim instance from this entity.
    /// </summary>
    /// <returns></returns>
    public virtual Claim ToClaim()
    {
        return new Claim(ClaimType, ClaimValue);
    }

    public virtual void SetClaim(Claim claim)
    {
        Check.NotNull(claim, nameof(claim));

        ClaimType = claim.Type;
        ClaimValue = claim.Value;
    }
}