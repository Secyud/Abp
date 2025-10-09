namespace Secyud.Abp.Permissions;

public class PermissionGroupInfo(string name)
{
    public string Name { get; set; } = name;

    public string? DisplayName { get; set; }
}