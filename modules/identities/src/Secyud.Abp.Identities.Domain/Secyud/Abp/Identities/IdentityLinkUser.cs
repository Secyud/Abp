using Volo.Abp.Domain.Entities;

namespace Secyud.Abp.Identities;

public class IdentityLinkUser : BasicAggregateRoot<Guid>
{
    public Guid SourceUserId { get; protected set; }

    public Guid? SourceTenantId { get; protected set; }

    public Guid TargetUserId { get; protected set; }

    public Guid? TargetTenantId { get; protected set; }

    /// <summary>
    /// Initializes a new instance of <see cref="IdentityLinkUser"/>.
    /// </summary>
    protected IdentityLinkUser()
    {

    }

    public IdentityLinkUser(Guid id, IdentityLinkUserInfo sourceUser, IdentityLinkUserInfo targetUser)
        : base(id)
    {
        SourceUserId = sourceUser.UserId;
        SourceTenantId = sourceUser.TenantId;

        TargetUserId = targetUser.UserId;
        TargetTenantId = targetUser.TenantId;
    }

    public IdentityLinkUser(Guid id, Guid sourceUserId, Guid? sourceTenantId, Guid targetUserId, Guid? targetTenantId)
        : base(id)
    {
        SourceUserId = sourceUserId;
        SourceTenantId = sourceTenantId;

        TargetUserId = targetUserId;
        TargetTenantId = targetTenantId;
    }
}
