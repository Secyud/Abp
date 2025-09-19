using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace SecitsDemoApp.EntityFrameworkCore;

[ConnectionStringName(SecitsDemoAppDbProperties.ConnectionStringName)]
public interface ISecitsDemoAppDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
