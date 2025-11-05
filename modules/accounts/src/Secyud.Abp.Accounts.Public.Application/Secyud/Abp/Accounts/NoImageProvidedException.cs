using Volo.Abp;

namespace Secyud.Abp.Accounts;

public class NoImageProvidedException : BusinessException
{
    public NoImageProvidedException()
    {
        Code = AccountProErrorCodes.NoImageProvided;
    }
}
