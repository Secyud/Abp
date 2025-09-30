using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Permissions.EntityFrameworkCore;

[ConnectionStringName(AbpPermissionsDbProperties.ConnectionStringName)]
public class PermissionsDbContext : AbpDbContext<PermissionsDbContext>, IPermissionsDbContext
{
    public DbSet<PermissionGroupDefinitionRecord> PermissionGroups { get; set; }
    public DbSet<PermissionDefinitionRecord> Permissions { get; set; }
    public DbSet<PermissionGrant> PermissionGrants { get; set; }

    public PermissionsDbContext(DbContextOptions<PermissionsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigurePermissions();
    }
}
