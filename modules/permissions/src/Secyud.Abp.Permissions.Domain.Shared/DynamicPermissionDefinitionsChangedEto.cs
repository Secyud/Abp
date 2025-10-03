using Volo.Abp.EventBus;

namespace Secyud.Abp.Permissions;

[Serializable]
[EventName("abp.permissions.dynamic-permission-definitions-changed")]
public class DynamicPermissionDefinitionsChangedEto
{
    public List<string> CreatedPermissions { get; set; } = [];
    public List<string> UpdatedPermissions { get; set; } = [];
    public List<string> DeletedPermissions { get; set; } = [];
}