using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Identities;

[Serializable]
public class IdentityUserEmailChangedEto : IMultiTenant
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string? Email { get; set; }

    public string? OldEmail { get; set; }
}