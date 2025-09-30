using Volo.Abp.Authorization.Permissions;

namespace Secyud.Abp.Permissions;

public interface IPermissionDefinitionSerializer
{
    Task<(PermissionGroupDefinitionRecord[], PermissionDefinitionRecord[])> SerializeAsync(
        IEnumerable<PermissionGroupDefinition> permissionGroups);

    Task<PermissionGroupDefinitionRecord> SerializeAsync(
        PermissionGroupDefinition permissionGroup);

    Task<PermissionDefinitionRecord> SerializeAsync(
        PermissionDefinition permission, PermissionGroupDefinition permissionGroup);
}