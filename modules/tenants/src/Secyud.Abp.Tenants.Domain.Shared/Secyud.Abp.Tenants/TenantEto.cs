using Volo.Abp.Auditing;

namespace Secyud.Abp.Tenants;

[Serializable]
public class TenantEto : IHasEntityVersion
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int EntityVersion { get; set; }
}