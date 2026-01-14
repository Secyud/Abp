namespace Secyud.Abp.GlobalFeatures;

public class GlobalFeatureManager
{
    public static GlobalFeatureManager Instance { get; protected set; } = new();

    /// <summary>
    /// A common dictionary to store arbitrary configurations.
    /// </summary>
    public Dictionary<object, object> Configuration { get; }

    public GlobalModuleFeaturesDictionary Modules { get; }

    protected HashSet<string> EnabledFeatures { get; }

    private GlobalFeatureManager()
    {
        EnabledFeatures = new HashSet<string>();
        Configuration = new Dictionary<object, object>();
        Modules = new GlobalModuleFeaturesDictionary(this);
    }

    public virtual bool IsEnabled<TFeature>()
    {
        return IsEnabled(typeof(TFeature));
    }

    public virtual bool IsEnabled(Type featureType)
    {
        return IsEnabled(GlobalFeatureNameAttribute.GetName(featureType));
    }

    public virtual bool IsEnabled(string featureName)
    {
        return EnabledFeatures.Contains(featureName);
    }

    public virtual void Enable<TFeature>()
    {
        Enable(typeof(TFeature));
    }

    public virtual void Enable(Type featureType)
    {
        Enable(GlobalFeatureNameAttribute.GetName(featureType));
    }

    public virtual void Enable(string featureName)
    {
        EnabledFeatures.AddIfNotContains(featureName);
    }

    public virtual void Disable<TFeature>()
    {
        Disable(typeof(TFeature));
    }

    public virtual void Disable(Type featureType)
    {
        Disable(GlobalFeatureNameAttribute.GetName(featureType));
    }

    public virtual void Disable(string featureName)
    {
        EnabledFeatures.Remove(featureName);
    }

    public virtual IEnumerable<string> GetEnabledFeatureNames()
    {
        return EnabledFeatures;
    }
}
