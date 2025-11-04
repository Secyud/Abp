using System.Security.Claims;

namespace Secyud.Abp.Identities;

/// <summary>
/// Represents a claim that a user possesses. 
/// </summary>
public class IdentityUserClaim : IdentityClaim
{
    /// <summary>
    /// Gets or sets the primary key of the user associated with this claim.
    /// </summary>
    public Guid UserId { get; protected set; }

    protected IdentityUserClaim()
    {

    }

    protected internal IdentityUserClaim(Guid id, Guid userId, Claim claim, Guid? tenantId)
        : base(id, claim, tenantId)
    {
        UserId = userId;
    }

    public IdentityUserClaim(Guid id, Guid userId, string claimType, string claimValue, Guid? tenantId)
        : base(id, claimType, claimValue, tenantId)
    {
        UserId = userId;
    }
}
