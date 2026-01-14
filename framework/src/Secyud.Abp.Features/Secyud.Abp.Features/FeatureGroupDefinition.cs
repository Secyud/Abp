using System.Collections.Immutable;
using Volo.Abp.Localization;

namespace Secyud.Abp.Features;

public class FeatureGroupDefinition(string name) : IWithFeatures
{
    private readonly List<FeatureDefinition> _features = [];

    /// <summary>
    /// Unique name of the group.
    /// </summary>
    public string Name { get; } = name;

    public Dictionary<string, object?> Properties { get; } = [];

    public required ILocalizableString DisplayName { get; set; }

    public IReadOnlyList<FeatureDefinition> Features => _features.ToImmutableList();

    /// <summary>
    /// Gets/sets a key-value on the <see cref="Properties"/>.
    /// </summary>
    /// <param name="name">Name of the property</param>
    /// <returns>
    /// Returns the value in the <see cref="Properties"/> dictionary by given <paramref name="name"/>.
    /// Returns null if given <paramref name="name"/> is not present in the <see cref="Properties"/> dictionary.
    /// </returns>
    public object? this[string name]
    {
        get => Properties.GetOrDefault(name);
        set => Properties[name] = value;
    }

    /// <summary>
    /// Sets a property in the <see cref="Properties"/> dictionary.
    /// This is a shortcut for nested calls on this object.
    /// </summary>
    public virtual FeatureGroupDefinition WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }

    public override string ToString()
    {
        return $"[{nameof(FeatureGroupDefinition)} {Name}]";
    }

    public FeatureDefinition AddFeature(string featureName, ILocalizableString? displayName = null)
    {
        var feature = new FeatureDefinition(featureName, this, null)
        {
            DisplayName = displayName ?? new FixedLocalizableString(
                FeatureDefinitionExtensions.CreateLocalizableStringKey(featureName))
        };
        
        _features.Add(feature);
        return feature;
    }

    public bool RemoveFeature(string permissionName, bool recurse)
    {
        var index = _features.FindIndex(u => u.Name == permissionName);
        if (index >= 0)
        {
            _features.RemoveAt(index);
            return true;
        }

        if (recurse)
        {
            foreach (var feature in _features)
            {
                if (feature.RemoveFeature(permissionName, true))
                {
                    return true;
                }
            }
        }

        return false;
    }
}