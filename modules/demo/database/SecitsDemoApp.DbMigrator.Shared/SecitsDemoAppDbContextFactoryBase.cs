using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SecitsDemoApp;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public abstract class SecitsDemoAppDbContextFactoryBase<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
    where TDbContext : DbContext
{
    public TDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<TDbContext>();

        return CreateDbContext(builder, configuration);
    }

    protected abstract TDbContext CreateDbContext(
        DbContextOptionsBuilder<TDbContext> builder,
        IConfigurationRoot configuration);

    protected IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}