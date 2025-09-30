using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace SecitsDemoApp.EntityFrameworkCore;

[ConnectionStringName(SecitsDemoAppDbProperties.ConnectionStringName)]
public class SecitsDemoAppDbContext(DbContextOptions<SecitsDemoAppDbContext> options) : AbpDbContext<SecitsDemoAppDbContext>(options), ISecitsDemoAppDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureSecitsDemoApp();
    }
}
