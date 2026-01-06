namespace Secyud.Abp.Authorization.Permissions;

public interface IPermissionDefinitionStore
{
    Task<IPermissionDefinition?> GetOrNullAsync(string name);

    Task<IReadOnlyList<IPermissionDefinition>> GetPermissionsAsync();
    Task<IReadOnlyList<IPermissionDefinition>> GetGroupsAsync();
}