namespace Secyud.Abp.Permissions;

public class PermissionGroupInfoDto
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public int TotalPermissionCount { get; set; }
    public int GrantPermissionCount { get; set; }
}