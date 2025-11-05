namespace Secyud.Abp.Accounts.ExternalProviders;

public class GetByNameInput
{
    public Guid? TenantId { get; set; }

    public required string Name { get; set; }
}
