namespace Secyud.Abp.Identities;

public class IdentityLinkUserInfo(Guid userId, Guid? tenantId = null)
{
    public Guid UserId { get; set; } = userId;

    public Guid? TenantId { get; set; } = tenantId;
}
