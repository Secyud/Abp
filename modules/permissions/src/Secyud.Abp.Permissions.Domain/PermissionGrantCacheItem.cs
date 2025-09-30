using Volo.Abp.Text.Formatting;

namespace Secyud.Abp.Permissions;

[Serializable]
public class PermissionGrantCacheItem
{
    public bool IsGranted { get; set; }

    public PermissionGrantCacheItem()
    {
    }

    public PermissionGrantCacheItem(bool isGranted)
    {
        IsGranted = isGranted;
    }
}