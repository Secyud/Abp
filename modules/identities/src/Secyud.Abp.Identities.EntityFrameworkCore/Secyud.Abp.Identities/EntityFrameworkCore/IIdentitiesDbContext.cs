using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Identities.EntityFrameworkCore;

[ConnectionStringName(AbpIdentitiesDbProperties.ConnectionStringName)]
public interface IIdentitiesDbContext : IEfCoreDbContext
{
    DbSet<IdentityUser> Users { get; }

    DbSet<IdentityRole> Roles { get; }

    DbSet<IdentityClaimType> ClaimTypes { get; }

    DbSet<IdentitySecurityLog> SecurityLogs { get; }

    DbSet<IdentityLinkUser> LinkUsers { get; }

    DbSet<IdentityUserDelegation> UserDelegations { get; }

    DbSet<IdentitySession> Sessions { get; }
}