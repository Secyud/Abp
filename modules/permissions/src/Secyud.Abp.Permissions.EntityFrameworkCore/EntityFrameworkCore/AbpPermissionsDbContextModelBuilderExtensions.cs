using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Secyud.Abp.Permissions.EntityFrameworkCore;

public static class AbpPermissionsDbContextModelBuilderExtensions
{
    public static void ConfigurePermissions(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<PermissionGrant>(b =>
        {
            b.ToTable(AbpPermissionsDbProperties.DbTablePrefix + "PermissionGrants", AbpPermissionsDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name).HasMaxLength(PermissionDefinitionRecordConsts.MaxNameLength).IsRequired();
            b.Property(x => x.ProviderName).HasMaxLength(PermissionGrantConsts.MaxProviderNameLength).IsRequired();
            b.Property(x => x.ProviderKey).HasMaxLength(PermissionGrantConsts.MaxProviderKeyLength).IsRequired();

            b.HasIndex(x => new { x.TenantId, x.Name, x.ProviderName, x.ProviderKey }).IsUnique();

            b.ApplyObjectExtensionMappings();
        });

        if (builder.IsHostDatabase())
        {
            builder.Entity<PermissionGroupDefinitionRecord>(b =>
            {
                b.ToTable(AbpPermissionsDbProperties.DbTablePrefix + "PermissionGroups",
                    AbpPermissionsDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).HasMaxLength(PermissionGroupDefinitionRecordConsts.MaxNameLength).IsRequired();
                b.Property(x => x.DisplayName).HasMaxLength(PermissionGroupDefinitionRecordConsts.MaxDisplayNameLength)
                    .IsRequired();

                b.HasIndex(x => new { x.Name }).IsUnique();

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<PermissionDefinitionRecord>(b =>
            {
                b.ToTable(AbpPermissionsDbProperties.DbTablePrefix + "Permissions",
                    AbpPermissionsDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.GroupName).HasMaxLength(PermissionGroupDefinitionRecordConsts.MaxNameLength)
                    .IsRequired();
                b.Property(x => x.Name).HasMaxLength(PermissionDefinitionRecordConsts.MaxNameLength).IsRequired();
                b.Property(x => x.ParentName).HasMaxLength(PermissionDefinitionRecordConsts.MaxNameLength);
                b.Property(x => x.DisplayName).HasMaxLength(PermissionDefinitionRecordConsts.MaxDisplayNameLength)
                    .IsRequired();
                b.Property(x => x.Providers).HasMaxLength(PermissionDefinitionRecordConsts.MaxProvidersLength);
                b.Property(x => x.StateCheckers).HasMaxLength(PermissionDefinitionRecordConsts.MaxStateCheckersLength);

                b.HasIndex(x => new { x.Name }).IsUnique();
                b.HasIndex(x => new { x.GroupName });

                b.ApplyObjectExtensionMappings();
            });
        }

        builder.TryConfigureObjectExtensions<PermissionsDbContext>();
    }
}
