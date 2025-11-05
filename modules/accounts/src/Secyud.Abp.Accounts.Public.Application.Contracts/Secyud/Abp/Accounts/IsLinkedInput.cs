namespace Secyud.Abp.Accounts;

public class IsLinkedInput
{
    public Guid UserId { get; set; }

    public Guid? TenantId { get; set; }
}
