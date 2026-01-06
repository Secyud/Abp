using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Authorization.Permissions;
using Volo.Abp;
using Volo.Abp.SimpleStateChecking;

namespace Secyud.Abp.Authorization.SimpleStateChecking;

public class RequirePermissionsSimpleBatchStateChecker<TState> : SimpleBatchStateCheckerBase<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    public static RequirePermissionsSimpleBatchStateChecker<TState> Current => CurrentAccessor.Value!;
    private static readonly AsyncLocal<RequirePermissionsSimpleBatchStateChecker<TState>> CurrentAccessor = new();

    private readonly List<RequirePermissionsSimpleBatchStateCheckerModel<TState>> _models = [];

    static RequirePermissionsSimpleBatchStateChecker()
    {
        CurrentAccessor.Value = new RequirePermissionsSimpleBatchStateChecker<TState>();
    }

    public RequirePermissionsSimpleBatchStateChecker<TState> AddCheckModels(
        params RequirePermissionsSimpleBatchStateCheckerModel<TState>[] models)
    {
        Check.NotNullOrEmpty(models, nameof(models));

        _models.AddRange(models);
        return this;
    }

    public static IDisposable Use(RequirePermissionsSimpleBatchStateChecker<TState> checker)
    {
        var previousValue = Current;
        CurrentAccessor.Value = checker;
        return new DisposeAction(() => CurrentAccessor.Value = previousValue);
    }

    public override async Task<SimpleStateCheckerResult<TState>> IsEnabledAsync(
        SimpleBatchStateCheckerContext<TState> context)
    {
        var permissionChecker = context.ServiceProvider.GetRequiredService<IPermissionChecker>();

        var result = new SimpleStateCheckerResult<TState>(context.States);

        var permissions = _models
            .Where(x =>
                context.States.Any(s => s.Equals(x.State)))
            .SelectMany(x => x.Permissions)
            .Distinct().ToArray();

        var grantResult = await permissionChecker.IsGrantedAsync(permissions);

        foreach (var state in context.States)
        {
            var model = _models
                .FirstOrDefault(x => x.State.Equals(state));
            if (model is null) continue;

            if (model.RequiresAll)
            {
                result[model.State] = model.Permissions.All(x =>
                    grantResult.Result.Any(y =>
                        y.Key == x && y.Value == PermissionGrantResult.Granted));
            }
            else
            {
                result[model.State] = grantResult.Result.Any(x =>
                    model.Permissions.Contains(x.Key) && x.Value == PermissionGrantResult.Granted);
            }
        }

        return result;
    }
}