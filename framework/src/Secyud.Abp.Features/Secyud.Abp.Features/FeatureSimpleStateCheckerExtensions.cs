using Volo.Abp;
using Volo.Abp.SimpleStateChecking;

namespace Secyud.Abp.Features;

public static class FeatureSimpleStateCheckerExtensions
{
    extension<TState>(TState state) where TState : IHasSimpleStateCheckers<TState>
    {
        public TState RequireFeatures(params string[] features)
        {
            state.RequireFeatures(true, features);
            return state;
        }

        public TState RequireFeatures(bool requiresAll, params string[] features)
        {
            Check.NotNull(state, nameof(state));
            Check.NotNullOrEmpty(features, nameof(features));

            state.StateCheckers.Add(new RequireFeaturesSimpleStateChecker<TState>(requiresAll, features));
            return state;
        }
    }
}
