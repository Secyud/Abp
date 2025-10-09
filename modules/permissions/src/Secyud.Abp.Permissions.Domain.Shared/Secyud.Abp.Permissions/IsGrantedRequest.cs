namespace Secyud.Abp.Permissions;

public class IsGrantedRequest
{
    public Guid UserId { get; set; }

    public string[] PermissionNames { get; set; } = null!;
}
