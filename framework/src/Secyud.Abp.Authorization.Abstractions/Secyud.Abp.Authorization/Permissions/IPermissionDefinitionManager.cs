namespace Secyud.Abp.Authorization.Permissions;

public interface IPermissionDefinitionManager
{
    Task<IPermissionDefinition> GetAsync(string name);

    Task<IPermissionDefinition?> GetOrNullAsync(string name);

    Task<IReadOnlyList<IPermissionDefinition>> GetPermissionsAsync();
    Task<IReadOnlyList<IPermissionDefinition>> GetGroupsAsync();
}