using Volo.Abp.Domain.Entities;

namespace Secyud.Abp.Tenants;

public class TenantUpdateDto : TenantCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; } = "";
}
