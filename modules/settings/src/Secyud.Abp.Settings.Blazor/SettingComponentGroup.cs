using JetBrains.Annotations;
using Volo.Abp;

namespace Secyud.Abp.Settings;

public class SettingComponentGroup(
    string id,
    string displayName,
    Type componentType,
    object? parameter = null,
    int order = SettingComponentGroup.DefaultOrder)
{
    public const int DefaultOrder = 1000;

    public string Id
    {
        get => _id;
        set => _id = Check.NotNullOrWhiteSpace(value, nameof(Id));
    }

    private string _id = id;

    public string DisplayName
    {
        get => _displayName;
        set => _displayName = Check.NotNullOrWhiteSpace(value, nameof(DisplayName));
    }

    private string _displayName = displayName;

    public Type ComponentType
    {
        get => _componentType;
        set => _componentType = Check.NotNull(value, nameof(ComponentType));
    }

    private Type _componentType = componentType;

    public object? Parameter { get; set; } = parameter;

    public int Order { get; set; } = order;
}