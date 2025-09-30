using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Permissions.EntityFrameworkCore;

[ConnectionStringName(AbpPermissionsDbProperties.ConnectionStringName)]
public interface IPermissionsDbContext : IEfCoreDbContext
{
    DbSet<PermissionGroupDefinitionRecord> PermissionGroups { get; }
    
    DbSet<PermissionDefinitionRecord> Permissions { get; }
    
    DbSet<PermissionGrant> PermissionGrants { get; }
}
