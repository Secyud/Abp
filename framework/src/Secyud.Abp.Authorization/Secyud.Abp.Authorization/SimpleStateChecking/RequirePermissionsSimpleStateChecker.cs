using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Authorization.Permissions;
using Volo.Abp.SimpleStateChecking;

namespace Secyud.Abp.Authorization.SimpleStateChecking;

public class RequirePermissionsSimpleStateChecker<TState>(
    RequirePermissionsSimpleBatchStateCheckerModel<TState> model)
    : ISimpleStateChecker<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    public bool RequiresAll => model.RequiresAll;

    public string[] PermissionNames => model.Permissions;

    public async Task<bool> IsEnabledAsync(SimpleStateCheckerContext<TState> context)
    {
        var permissionChecker = context.ServiceProvider.GetRequiredService<IPermissionChecker>();

        if (model.Permissions.Length == 1)
        {
            var result = await permissionChecker.IsGrantedAsync(model.Permissions.First());
            return result == PermissionGrantResult.Granted;
        }

        var grantResult = await permissionChecker.IsGrantedAsync(model.Permissions);

        return model.RequiresAll
            ? grantResult.AllGranted
            : grantResult.Result.Any(x =>
                model.Permissions.Contains(x.Key) && x.Value == PermissionGrantResult.Granted);
    }
}