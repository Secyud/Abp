namespace Secyud.Abp.Permissions;

public class PermissionGrantInfo(string name, bool isGranted)
{
    public string Name { get; } = name;
    public string? ParentName { get; set; }
    public bool IsGranted { get; set; } = isGranted;
    public string? DisplayName { get; set; }

    /// <summary>
    /// Comma separated list of provider names.
    /// </summary>
    public List<string>? Providers { get; set; }
}