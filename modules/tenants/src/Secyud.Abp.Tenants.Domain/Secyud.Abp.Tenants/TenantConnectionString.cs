using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Secyud.Abp.Tenants;

public class TenantConnectionString : Entity
{
    public Guid TenantId { get; protected set; }

    public string Name { get; protected set; } = string.Empty;

    public string Value { get; protected set; } = string.Empty;

    protected TenantConnectionString()
    {
    }

    public TenantConnectionString(Guid tenantId, string name, string value)
    {
        TenantId = tenantId;
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), TenantConnectionStringConsts.MaxNameLength);
        SetValue(value);
    }

    public void SetValue(string value)
    {
        Value = Check.NotNullOrWhiteSpace(value, nameof(value), TenantConnectionStringConsts.MaxValueLength);
    }

    public override object[] GetKeys()
    {
        return new object[] { TenantId, Name };
    }
}