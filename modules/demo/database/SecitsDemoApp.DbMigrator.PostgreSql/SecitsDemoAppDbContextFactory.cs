using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SecitsDemoApp.EntityFrameworkCore;

namespace SecitsDemoApp;

public class SecitsDemoAppDbContextFactory :
    SecitsDemoAppDbContextFactoryBase<SecitsDemoAppMigratorDbContext>
{

    protected override SecitsDemoAppMigratorDbContext CreateDbContext(
        DbContextOptionsBuilder<SecitsDemoAppMigratorDbContext> builder,
        IConfigurationRoot configuration)
    {
        builder = builder.UseNpgsql(configuration["ConnectionStrings:Default"]);
        
        return new SecitsDemoAppMigratorDbContext(builder.Options);
    }
}