using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Tenants.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpTenantsDbProperties.ConnectionStringName)]
public class TenantsDbContext(DbContextOptions<TenantsDbContext> options) : AbpDbContext<TenantsDbContext>(options), ITenantsDbContext
{
    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureTenants();
    }
}
