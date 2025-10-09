namespace Secyud.Abp.Permissions;

public class IsGrantedResponse
{
    public Guid UserId { get; set; }

    public Dictionary<string, bool> Permissions { get; set; } = [];
}
