using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace Secyud.Abp.Accounts;

public class ProfileDto : ExtensibleObject, IHasConcurrencyStamp
{
    public string UserName { get; set; } = "";

    public string Email { get; set; } = "";

    public bool EmailConfirmed { get; set; }

    public string? Name { get; set; } 

    public string? Surname { get; set; } 

    public string? PhoneNumber { get; set; } 

    public bool PhoneNumberConfirmed { get; set; }

    public bool IsExternal { get; set; }

    public bool HasPassword { get; set; }

    public bool SupportsMultipleTimezone { get; set; }

    public string? Timezone { get; set; }

    public string ConcurrencyStamp { get; set; } = "";
}