using System.Collections.Immutable;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Secyud.Abp.Features;

public class FeatureDefinition : IWithFeatures
{
    private readonly List<FeatureDefinition> _children = [];

    internal FeatureDefinition(string name, FeatureGroupDefinition group, FeatureDefinition? parent)
    {
        Name = name;
        Group = group;
        Parent = parent;
    }

    public FeatureGroupDefinition Group { get; }
    public FeatureDefinition? Parent { get; }

    /// <summary>
    /// Unique name of the feature.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Default value of the feature.
    /// </summary>
    public string? DefaultValue { get; set; }

    public required ILocalizableString DisplayName { get; set; }

    public ILocalizableString? Description { get; set; }

    /// <summary>
    /// Can clients see this feature and it's value.
    /// Default: true.
    /// </summary>
    public bool IsVisibleToClients { get; set; } = true;

    /// <summary>
    /// Can host use this feature.
    /// Default: true.
    /// </summary>
    public bool IsAvailableToHost { get; set; } = true;

    /// <summary>
    /// A list of allowed providers to get/set value of this feature.
    /// An empty list indicates that all providers are allowed.
    /// </summary>
    public List<string> AllowedProviders { get; } = [];

    /// <summary>
    /// List of child features.
    /// </summary>
    public IReadOnlyList<FeatureDefinition> Children => _children.ToImmutableList();

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
    /// Can be used to get/set custom properties for this feature.
    /// </summary>
    public Dictionary<string, object?> Properties { get; } = [];

    /// <summary>
    /// Input type.
    /// This can be used to prepare an input for changing this feature's value.
    /// Default: <see cref="ToggleStringValueType"/>.
    /// </summary>
    public IStringValueType? ValueType { get; set; }


    /// <summary>
    /// Sets a property in the <see cref="Properties"/> dictionary.
    /// This is a shortcut for nested calls on this object.
    /// </summary>
    public virtual FeatureDefinition WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }

    /// <summary>
    /// Adds one or more providers to the <see cref="AllowedProviders"/> list.
    /// This is a shortcut for nested calls on this object.
    /// </summary>
    public virtual FeatureDefinition WithProviders(params string[] providers)
    {
        if (!providers.IsNullOrEmpty())
        {
            AllowedProviders.AddRange(providers);
        }

        return this;
    }

    public override string ToString()
    {
        return $"[{nameof(FeatureDefinition)}: {Name}]";
    }

    public FeatureDefinition AddFeature(string featureName, ILocalizableString? displayName = null)
    {
        var feature = new FeatureDefinition(featureName, Group, this)
        {
            DisplayName = displayName ?? new FixedLocalizableString(
                FeatureDefinitionExtensions.CreateLocalizableStringKey(featureName)),
        };
        _children.Add(feature);
        return feature;
    }

    public bool RemoveFeature(string featureName, bool recurse)
    {
        if (!recurse)
        {
            var index = _children.FindIndex(x => x.Name == featureName);
            if (index < 0) return false;
            _children.RemoveAt(index);
            return true;
        }

        var queue = new Queue<List<FeatureDefinition>>();
        queue.Enqueue(_children);

        while (queue.Count > 0)
        {
            var list = queue.Dequeue();

            for (var i = 0; i < list.Count; i++)
            {
                var feature = list[i];
                if (feature.Name == featureName)
                {
                    list.RemoveAt(i);
                    return true;
                }

                if (feature._children.Count > 0)
                {
                    queue.Enqueue(list);
                }
            }
        }

        return false;
    }
}