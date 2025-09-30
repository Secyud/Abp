using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Settings.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpSettingsDbProperties.ConnectionStringName)]
public interface ISettingsDbContext : IEfCoreDbContext
{
    DbSet<Setting> Settings { get; }

    DbSet<SettingDefinitionRecord> SettingDefinitionRecords { get; }
}
