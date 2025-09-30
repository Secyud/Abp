using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Settings.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpSettingsDbProperties.ConnectionStringName)]
public class SettingsDbContext(DbContextOptions<SettingsDbContext> options) : AbpDbContext<SettingsDbContext>(options), ISettingsDbContext
{
    public DbSet<Setting> Settings { get; set; }
    public DbSet<SettingDefinitionRecord> SettingDefinitionRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureSettings();
    }
}
