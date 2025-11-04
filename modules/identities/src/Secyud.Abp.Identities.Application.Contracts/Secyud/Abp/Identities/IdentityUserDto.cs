using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Identities;

public class IdentityUserDto : ExtensibleFullAuditedEntityDto<Guid>, IMultiTenant, IHasConcurrencyStamp
{
    public Guid? TenantId { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool SupportTwoFactor { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public bool IsActive { get; set; }

    public bool LockoutEnabled { get; set; }

    public bool IsLockedOut { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool ShouldChangePasswordOnNextLogin { get; set; }

    public string ConcurrencyStamp { get; set; } = "";

    public List<string> RoleNames { get; set; } = [];

    public int AccessFailedCount { get; set; }

    public DateTimeOffset? LastPasswordChangeTime { get; set; }
    
    public bool IsExternal { get; set; }
}
