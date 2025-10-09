using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Tenants.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpTenantsDbProperties.ConnectionStringName)]
public class TenantsDbContext : AbpDbContext<TenantsDbContext>, ITenantsDbContext
{
    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    public TenantsDbContext(DbContextOptions<TenantsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureTenants();
    }
}
