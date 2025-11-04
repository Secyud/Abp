using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Identities;

[Serializable]
public class IdentityRoleEto : IMultiTenant, IHasEntityVersion
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsDefault { get; set; }

    public bool IsStatic { get; set; }

    public bool IsPublic { get; set; }

    public int EntityVersion { get; set; }
}