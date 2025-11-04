using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Identities;

public class IdentityRoleLookupDto : EntityDto<Guid>
{
    public string? Name { get; set; }
}
