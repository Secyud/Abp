using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Identities;

public class IdentitySessionDto : ExtensibleEntityDto<Guid>
{
    public string? SessionId { get; set; }

    public virtual bool IsCurrent { get; set; }

    public string? Device { get; set; }

    public string? DeviceInfo { get; set; }

    public virtual Guid? TenantId { get; set; }

    public string? TenantName { get; set; }

    public virtual Guid UserId { get; set; }

    public string? UserName { get; set; }

    public string? ClientId { get; set; }

    public string[] IpAddresses { get; set; } = [];

    public virtual DateTime SignedIn { get; set; }

    public virtual DateTime? LastAccessed { get; set; }
}