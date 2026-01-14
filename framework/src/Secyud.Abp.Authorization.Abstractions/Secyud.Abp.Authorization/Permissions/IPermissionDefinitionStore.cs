namespace Secyud.Abp.Authorization.Permissions;

public interface IPermissionDefinitionStore
{
    Task<PermissionDefinition?> GetOrNullAsync(string name);
    Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync();
    Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync();
}