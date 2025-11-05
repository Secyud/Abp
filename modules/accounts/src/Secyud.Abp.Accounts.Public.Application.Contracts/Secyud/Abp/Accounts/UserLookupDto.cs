using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Accounts;

public class UserLookupDto: EntityDto<Guid>
{
    public string UserName { get; set; }= "";
}