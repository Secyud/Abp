using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Accounts;

public class GetAccountIdentitySessionListInput : ExtensiblePagedAndSortedResultRequestDto
{
    public string Device { get; set; }= "";

    public string ClientId { get; set; }= "";
}
