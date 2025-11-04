using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Identities;

public class GetIdentityUsersInput : ExtensiblePagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public Guid? RoleId { get; set; } = null;

    public Guid? OrganizationUnitId { get; set; } = null;

    public string? UserName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? EmailAddress { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public bool? IsLockedOut { get; set; }

    public bool? NotActive { get; set; }

    public bool? EmailConfirmed { get; set; }

    public bool? IsExternal { get; set; }

    public DateTime? MaxCreationTime { get; set; }

    public DateTime? MinCreationTime { get; set; }

    public DateTime? MaxModifitionTime { get; set; }

    public DateTime? MinModifitionTime { get; set; }
}
