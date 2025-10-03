using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Tenants.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpTenantsDbProperties.ConnectionStringName)]
public interface ITenantsDbContext : IEfCoreDbContext
{
    DbSet<Tenant> Tenants { get; }

    DbSet<TenantConnectionString> TenantConnectionStrings { get; }
}
