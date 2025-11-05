using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Accounts;

public class UserDelegationDto : EntityDto<Guid>
{
    public string UserName { get; set; } = "";

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}
