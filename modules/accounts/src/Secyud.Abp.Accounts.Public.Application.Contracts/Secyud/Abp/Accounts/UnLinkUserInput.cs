namespace Secyud.Abp.Accounts;

public class UnLinkUserInput
{
    public Guid UserId { get; set; }

    public Guid? TenantId { get; set; }
}
