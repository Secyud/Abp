namespace Secyud.Abp.Permissions;

public class PermissionGrantInfoDto
{
    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string? ParentName { get; set; }
    public string? GroupName { get; set; }
    public bool IsGranted { get; set; }
    public List<string> AllowedProviders { get; set; } = [];
}