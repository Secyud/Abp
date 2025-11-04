using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Secyud.Abp.Identities;

public class ClaimTypeDto : ExtensibleEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; } = "";

    public bool Required { get; set; }

    public bool IsStatic { get; set; }

    public string? Regex { get; set; }

    public string? RegexDescription { get; set; }

    public string? Description { get; set; }

    public IdentityClaimValueType ValueType { get; set; }

    public string ValueTypeAsString { get; set; } = "";

    public string ConcurrencyStamp { get; set; } = "";
}