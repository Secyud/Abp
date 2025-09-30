using JetBrains.Annotations;
using Volo.Abp;

namespace Secyud.Abp.Permissions;

public class PermissionGrantInfo(string name, bool isGranted)
{
    public string Name { get; } = name;
    public bool IsGranted { get; set; } = isGranted;
    public string? GroupName { get; set; }
    public string? ParentName { get; set; }
    public string? DisplayName { get; set; }
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Comma separated list of provider names.
    /// </summary>
    public List<string>? Providers { get; set; }
}