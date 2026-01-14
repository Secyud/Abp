using Volo.Abp;
using Volo.Abp.SimpleStateChecking;

namespace Secyud.Abp.GlobalFeatures;

public static class GlobalFeatureSimpleStateCheckerExtensions
{
    extension<TState>(TState state) where TState : IHasSimpleStateCheckers<TState>
    {
        public TState RequireGlobalFeatures(params string[] globalFeatures)
        {
            return state.RequireGlobalFeatures(true, globalFeatures);
        }

        public TState RequireGlobalFeatures(bool requiresAll,
            params string[] globalFeatures)
        {
            Check.NotNull(state, nameof(state));
            Check.NotNullOrEmpty(globalFeatures, nameof(globalFeatures));

            state.StateCheckers.Add(new RequireGlobalFeaturesSimpleStateChecker<TState>(requiresAll, globalFeatures));
            return state;
        }

        public TState RequireGlobalFeatures(params Type[] globalFeatures)
        {
            return state.RequireGlobalFeatures(true, globalFeatures);
        }

        public TState RequireGlobalFeatures(bool requiresAll,
            params Type[] globalFeatures)
        {
            Check.NotNull(state, nameof(state));
            Check.NotNullOrEmpty(globalFeatures, nameof(globalFeatures));

            state.StateCheckers.Add(new RequireGlobalFeaturesSimpleStateChecker<TState>(requiresAll, globalFeatures));
            return state;
        }
    }
}
