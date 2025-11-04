using Volo.Abp.Domain.Entities;

namespace Secyud.Abp.Identities;

public class IdentityUserUpdateDto : IdentityUserCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    public bool EmailConfirmed { get; set; }
        
    public bool PhoneNumberConfirmed { get; set; }

    public string ConcurrencyStamp { get; set; } = "";
}
