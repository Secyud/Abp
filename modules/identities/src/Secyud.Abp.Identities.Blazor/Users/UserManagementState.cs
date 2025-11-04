using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Identities.Users;

public class UserManagementState : IScopedDependency
{
    public Func<GetIdentityUsersInput>? OnGetFilter { get; set; }
    public Func<Task>? OnDataGridChanged { get; set; }

    public GetIdentityUsersInput GetFilter()
    {
        return OnGetFilter?.Invoke() ?? new GetIdentityUsersInput();
    }

    public async Task DataGridChangedAsync()
    {
        if (OnDataGridChanged is not null)
            await OnDataGridChanged.Invoke();
    }
}