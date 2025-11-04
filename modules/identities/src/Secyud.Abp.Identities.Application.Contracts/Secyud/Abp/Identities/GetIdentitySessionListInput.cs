using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Identities;

public class GetIdentitySessionListInput : ExtensiblePagedAndSortedResultRequestDto
{
    public Guid UserId { get; set; }

    public string? Device { get; set; }

    public string? ClientId { get; set; }
}
