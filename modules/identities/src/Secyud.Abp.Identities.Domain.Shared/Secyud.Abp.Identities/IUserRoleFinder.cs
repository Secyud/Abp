namespace Secyud.Abp.Identities;

public interface IUserRoleFinder
{
    Task<string[]> GetRoleNamesAsync(Guid userId);
}