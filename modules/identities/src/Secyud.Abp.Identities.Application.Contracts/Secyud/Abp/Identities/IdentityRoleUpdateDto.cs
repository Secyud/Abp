using Volo.Abp.Domain.Entities;

namespace Secyud.Abp.Identities;

public class IdentityRoleUpdateDto : IdentityRoleCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }= string.Empty;
}
