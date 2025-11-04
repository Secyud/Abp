using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Identities;

public class GetIdentityRoleListInput : ExtensiblePagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
