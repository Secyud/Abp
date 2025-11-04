namespace Secyud.Abp.Identities.Users;

public class AdvancedFilterInput
{
    public Guid? OrganizationUnitId { get; set; }
    public Guid? RoleId { get; set; }
    public bool? NotActive { get; set; }
    public bool? EmailConfirmed { get; set; }
    public bool? IsLockedOut { get; set; }
    public bool? IsExternal { get; set; }
}