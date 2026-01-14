namespace Secyud.Abp.Permissions.Components;

public class PermissionGroupInfoModel
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public int TotalPermissionCount { get; set; }
    public int GrantPermissionCount { get; set; }
    public List<PermissionGrantInfoModel>? Permissions { get; set; }
    public List<PermissionGrantInfoModel>? RootPermissions { get; set; }
}