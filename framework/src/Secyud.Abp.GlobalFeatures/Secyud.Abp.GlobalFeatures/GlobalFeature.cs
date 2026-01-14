using Volo.Abp;

namespace Secyud.Abp.GlobalFeatures;

public abstract class GlobalFeature
{
    public GlobalModuleFeatures Module { get; }

    public GlobalFeatureManager FeatureManager { get; }

    public string FeatureName { get; }

    public bool IsEnabled {
        get => FeatureManager.IsEnabled(FeatureName);
        set => SetEnabled(value);
    }

    protected GlobalFeature(GlobalModuleFeatures module)
    {
        Module = Check.NotNull(module, nameof(module));
        FeatureManager = Module.FeatureManager;
        FeatureName = GlobalFeatureNameAttribute.GetName(GetType());
    }

    public virtual void Enable()
    {
        FeatureManager.Enable(FeatureName);
    }

    public virtual void Disable()
    {
        FeatureManager.Disable(FeatureName);
    }

    public void SetEnabled(bool isEnabled)
    {
        if (isEnabled)
        {
            Enable();
        }
        else
        {
            Disable();
        }
    }
}
