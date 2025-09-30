using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Permissions;

public class PermissionGrant : Entity<Guid>, IMultiTenant
{
    protected PermissionGrant()
    {
    }

    public PermissionGrant(Guid id, string name, string providerName,
        string providerKey, Guid? tenantId = null) : base(id)
    {
        TenantId = tenantId;
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        ProviderName = Check.NotNullOrWhiteSpace(providerName, nameof(providerName));
        ProviderKey = providerKey;
    }

    public Guid? TenantId { get; protected set; }

    public string Name { get; protected set; } = "";

    public string ProviderName { get; protected set; } = "";

    public string ProviderKey { get; protected set; } = "";
}