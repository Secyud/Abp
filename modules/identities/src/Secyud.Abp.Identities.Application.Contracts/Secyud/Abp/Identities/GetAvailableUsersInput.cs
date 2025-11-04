using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Identities;

public class GetAvailableUsersInput : ExtensiblePagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public Guid Id { get; set; }
}
