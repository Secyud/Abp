using Volo.Abp.Users;

namespace Secyud.Abp.Users;

public interface IUpdateUserData
{
    bool Update(IUserData user);
}
