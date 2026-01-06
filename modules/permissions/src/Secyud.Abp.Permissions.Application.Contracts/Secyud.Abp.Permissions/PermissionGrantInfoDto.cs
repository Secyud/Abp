namespace Secyud.Abp.Permissions;

public class PermissionGrantInfoDto
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? ParentName { get; set; }
    public bool IsGranted { get; set; }
}