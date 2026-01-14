using Volo.Abp;

namespace Secyud.Abp.GlobalFeatures;

[AttributeUsage(AttributeTargets.Class)]
public class RequiresGlobalFeatureAttribute : Attribute
{
    public Type? Type { get; }

    public string? Name { get; }

    public RequiresGlobalFeatureAttribute(Type type)
    {
        Type = Check.NotNull(type, nameof(type));
    }

    public RequiresGlobalFeatureAttribute(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
    }

    public virtual string GetFeatureName()
    {
        return Name ?? GlobalFeatureNameAttribute.GetName(Type!);
    }
}
