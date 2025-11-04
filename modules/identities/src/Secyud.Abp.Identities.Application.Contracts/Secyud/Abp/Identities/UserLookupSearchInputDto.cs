using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Identities;

public class UserLookupSearchInputDto : ExtensiblePagedResultRequestDto, ISortedResultRequest
{
    public string? Sorting { get; set; }

    public string? Filter { get; set; }
}