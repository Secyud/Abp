using System.Reflection;
using Volo.Abp;

namespace Secyud.Abp.GlobalFeatures;

[AttributeUsage(AttributeTargets.Class)]
public class GlobalFeatureNameAttribute(string name) : Attribute
{
    public string Name { get; } = Check.NotNullOrWhiteSpace(name, nameof(name));

    public static string GetName<TFeature>()
    {
        return GetName(typeof(TFeature));
    }

    public static string GetName(Type type)
    {
        Check.NotNull(type, nameof(type));

        var attribute = type
            .GetCustomAttributes<GlobalFeatureNameAttribute>()
            .FirstOrDefault();

        if (attribute == null)
        {
            throw new AbpException($"{type.AssemblyQualifiedName} should define the {typeof(GlobalFeatureNameAttribute).FullName} atttribute!");
        }

        return attribute
            .As<GlobalFeatureNameAttribute>()
            .Name;
    }
}
