using Microsoft.EntityFrameworkCore;
using Secyud.Abp.Features.EntityFrameworkCore;
using Secyud.Abp.Permissions.EntityFrameworkCore;
using Secyud.Abp.Settings.EntityFrameworkCore;
using Secyud.Abp.Tenants.EntityFrameworkCore;
using Volo.Abp;

namespace SecitsDemoApp.EntityFrameworkCore;

public static class SecitsDemoAppDbContextModelCreatingExtensions
{
    public static void ConfigureSecitsDemoApp(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
        
        builder.ConfigureFeatures();
        builder.ConfigureSettings();
        builder.ConfigureTenants();
        builder.ConfigurePermissions();
    }
}
