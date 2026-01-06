using System.Globalization;
using Secyud.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;

namespace Secyud.Abp.Permissions;

public class PermissionDefinitionSerializer(
    IGuidGenerator guidGenerator,
    ILocalizableStringSerializer localizableStringSerializer)
    : IPermissionDefinitionSerializer, ITransientDependency
{
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; } = localizableStringSerializer;

    public Task<PermissionDefinitionRecord> SerializeAsync(IPermissionDefinition permission)
    {
        using var use = CultureHelper.Use(CultureInfo.InvariantCulture);
        var displayName = LocalizableStringSerializer.Serialize(permission.DisplayName)!;

        var permissionRecord = new PermissionDefinitionRecord(
            GuidGenerator.Create()
        )
        {
            ParentName = permission.ParentName,
            Name = permission.Name,
            DisplayName = displayName,
            MultiTenancySide = permission.MultiTenancySide,
            Providers = SerializeProviders(permission.Providers),
        };

        foreach (var property in permission.Properties)
        {
            permissionRecord.SetProperty(property.Key, property.Value);
        }

        return Task.FromResult(permissionRecord);
    }

    protected virtual string? SerializeProviders(ICollection<string> providers)
    {
        return providers.Count != 0
            ? providers.JoinAsString(",")
            : null;
    }
}