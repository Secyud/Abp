using Secyud.Secits.Blazor.Settings;

namespace Secyud.Abp.Permissions.Components;

public class PermissionGrantInfoModel : PermissionGrantInfoDto, ITreeItem<PermissionGrantInfoModel>
{
    public int Index { get; set; }
    public int Depth { get; set; }

    public bool IsExpended { get; set; }
    public bool IsChecked { get; set; }
    public PermissionGroupInfoModel? Group { get; set; }
    public PermissionGrantInfoModel? Parent { get; set; }
    public List<PermissionGrantInfoModel>? Children { get; set; }
}