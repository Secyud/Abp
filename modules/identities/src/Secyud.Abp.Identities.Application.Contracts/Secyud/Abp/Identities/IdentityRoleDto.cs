using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Secyud.Abp.Identities;

public class IdentityRoleDto : ExtensibleEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; } = string.Empty;

    public bool IsDefault { get; set; }

    public bool IsStatic { get; set; }

    public bool IsPublic { get; set; }

    public long UserCount { get; set; }

    public string ConcurrencyStamp { get; set; } = string.Empty;
}