namespace Secyud.Abp.Permissions.Components;

public class PermissionGroupInfoModel : PermissionGroupInfoDto
{
    public int SelectedCount { get; set; }

    public int TotalCount { get; set; }
    public List<PermissionGrantInfoModel>? Permissions { get; set; }
    public List<PermissionGrantInfoModel>? RootPermissions { get; set; }
}