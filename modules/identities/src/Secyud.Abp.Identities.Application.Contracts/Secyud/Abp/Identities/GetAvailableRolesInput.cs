using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Identities;

public class GetAvailableRolesInput : ExtensiblePagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public Guid Id { get; set; }
}
