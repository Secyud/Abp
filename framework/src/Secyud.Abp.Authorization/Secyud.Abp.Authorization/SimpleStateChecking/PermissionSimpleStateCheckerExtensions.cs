using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.SimpleStateChecking;

namespace Secyud.Abp.Authorization.SimpleStateChecking;

public static class PermissionSimpleStateCheckerExtensions
{
    extension<TState>(TState state) where TState : IHasSimpleStateCheckers<TState>
    {
        public TState RequireAuthenticated()
        {
            state.StateCheckers.Add(new RequireAuthenticatedSimpleStateChecker<TState>());
            return state;
        }

        public TState RequirePermissions(params string[] permissions)
        {
            state.RequirePermissions(requiresAll: true, batchCheck: true, permissions);
            return state;
        }

        public TState RequirePermissions(bool requiresAll,
            params string[] permissions)
        {
            state.RequirePermissions(requiresAll: requiresAll, batchCheck: true, permissions);
            return state;
        }

        public TState RequirePermissions(bool requiresAll,
            bool batchCheck, params string[] permissions)
        {
            Check.NotNull(state, nameof(state));
            Check.NotNullOrEmpty(permissions, nameof(permissions));

            var models = new RequirePermissionsSimpleBatchStateCheckerModel<TState>(state, permissions, requiresAll);
            if (batchCheck)
            {
                var current = RequirePermissionsSimpleBatchStateChecker<TState>.Current;
                current.AddCheckModels(models);
                state.StateCheckers.Add(current);
            }
            else
            {
                state.StateCheckers.Add(new RequirePermissionsSimpleStateChecker<TState>(models));
            }

            return state;
        }
    }
}