using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Identities;

public class GetIdentityClaimTypesInput : ExtensiblePagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
