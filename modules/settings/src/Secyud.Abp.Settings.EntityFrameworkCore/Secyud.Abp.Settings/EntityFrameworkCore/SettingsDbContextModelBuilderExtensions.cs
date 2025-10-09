using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Secyud.Abp.Settings.EntityFrameworkCore;

public static class SettingsDbContextModelBuilderExtensions
{
    public static void ConfigureSettings(this ModelBuilder builder)
    {
        if (builder.IsTenantOnlyDatabase())
        {
            return;
        }

        builder.Entity<Setting>(b =>
        {
            b.ToTable(AbpSettingsDbProperties.DbTablePrefix + "Settings", AbpSettingsDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name).HasMaxLength(SettingConsts.MaxNameLength).IsRequired();

            if (builder.IsUsingOracle())
            {
                SettingConsts.MaxValueLengthValue = 2000;
            }

            b.Property(x => x.Value).HasMaxLength(SettingConsts.MaxValueLengthValue).IsRequired();

            b.Property(x => x.ProviderName).HasMaxLength(SettingConsts.MaxProviderNameLength);
            b.Property(x => x.ProviderKey).HasMaxLength(SettingConsts.MaxProviderKeyLength);

            b.HasIndex(x => new { x.Name, x.ProviderName, x.ProviderKey }).IsUnique(true);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<SettingDefinitionRecord>(b =>
        {
            b.ToTable(AbpSettingsDbProperties.DbTablePrefix + "SettingDefinitions", AbpSettingsDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name).HasMaxLength(SettingDefinitionRecordConsts.MaxNameLength).IsRequired();
            b.Property(x => x.DisplayName).HasMaxLength(SettingDefinitionRecordConsts.MaxDisplayNameLength).IsRequired();
            b.Property(x => x.Description).HasMaxLength(SettingDefinitionRecordConsts.MaxDescriptionLength);
            b.Property(x => x.DefaultValue).HasMaxLength(SettingDefinitionRecordConsts.MaxDefaultValueLength);
            b.Property(x => x.Providers).HasMaxLength(SettingDefinitionRecordConsts.MaxProvidersLength);

            b.HasIndex(x => new { x.Name }).IsUnique();

            b.ApplyObjectExtensionMappings();
        });

        builder.TryConfigureObjectExtensions<SettingsDbContext>();
    }
}