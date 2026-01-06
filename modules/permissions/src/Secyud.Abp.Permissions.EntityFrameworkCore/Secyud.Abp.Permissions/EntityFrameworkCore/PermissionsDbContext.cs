using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Permissions.EntityFrameworkCore;

[ConnectionStringName(AbpPermissionsDbProperties.ConnectionStringName)]
public class PermissionsDbContext(DbContextOptions<PermissionsDbContext> options) : AbpDbContext<PermissionsDbContext>(options), IPermissionsDbContext
{
    public DbSet<PermissionDefinitionRecord> Permissions { get; set; }
    public DbSet<PermissionGrant> PermissionGrants { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigurePermissions();
    }
}
