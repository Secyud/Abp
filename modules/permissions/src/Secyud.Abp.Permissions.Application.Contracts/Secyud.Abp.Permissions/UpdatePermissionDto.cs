namespace Secyud.Abp.Permissions;

public class UpdatePermissionDto
{
    public string Name { get; set; } = string.Empty;

    public bool IsGranted { get; set; }
}
