using Secyud.Abp.Authorization.Permissions;

namespace Secyud.Abp.Permissions;

public interface IPermissionDefinitionSerializer
{
    Task<PermissionDefinitionRecord> SerializeAsync(IPermissionDefinition permission);
}