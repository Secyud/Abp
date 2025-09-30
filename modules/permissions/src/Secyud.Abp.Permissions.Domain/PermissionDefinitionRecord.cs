using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Permissions;

public class PermissionDefinitionRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
    public PermissionDefinitionRecord()
    {
        this.SetDefaultsForExtraProperties();
    }

    public PermissionDefinitionRecord(Guid id,
        string groupName,
        string name,
        string? parentName,
        string displayName,
        bool isDynamic,
        bool isEnabled = true,
        MultiTenancySides multiTenancySide = MultiTenancySides.Both,
        string? providers = null,
        string? stateCheckers = null) : base(id)
    {
        GroupName = Check.NotNullOrWhiteSpace(groupName, nameof(groupName), PermissionGroupDefinitionRecordConsts.MaxNameLength);
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), PermissionDefinitionRecordConsts.MaxNameLength);
        ParentName = Check.Length(parentName, nameof(parentName), PermissionDefinitionRecordConsts.MaxNameLength);
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), PermissionDefinitionRecordConsts.MaxDisplayNameLength);
        IsDynamic = isDynamic;
        IsEnabled = isEnabled;
        MultiTenancySide = multiTenancySide;
        Providers = providers;
        StateCheckers = stateCheckers;
        this.SetDefaultsForExtraProperties();
    }

    public string GroupName { get; set; } = "";

    public string Name { get; set; } = "";

    public string? ParentName { get; set; }

    public string DisplayName { get; set; } = "";

    public bool IsEnabled { get; set; }
    public bool IsDynamic { get; set; }

    public MultiTenancySides MultiTenancySide { get; set; }

    /// <summary>
    /// Comma separated list of provider names.
    /// </summary>
    public string? Providers { get; set; }

    /// <summary>
    /// Serialized string to store info about the state checkers.
    /// </summary>
    public string? StateCheckers { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; protected set; } = [];

    public bool HasSameData(PermissionDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            return false;
        }

        if (GroupName != otherRecord.GroupName)
        {
            return false;
        }

        if (ParentName != otherRecord.ParentName)
        {
            return false;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            return false;
        }

        if (IsEnabled != otherRecord.IsEnabled)
        {
            return false;
        }

        if (MultiTenancySide != otherRecord.MultiTenancySide)
        {
            return false;
        }

        if (Providers != otherRecord.Providers)
        {
            return false;
        }

        if (StateCheckers != otherRecord.StateCheckers)
        {
            return false;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            return false;
        }

        return true;
    }

    public void Patch(PermissionDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            Name = otherRecord.Name;
        }

        if (GroupName != otherRecord.GroupName)
        {
            GroupName = otherRecord.GroupName;
        }

        if (ParentName != otherRecord.ParentName)
        {
            ParentName = otherRecord.ParentName;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            DisplayName = otherRecord.DisplayName;
        }

        if (IsEnabled != otherRecord.IsEnabled)
        {
            IsEnabled = otherRecord.IsEnabled;
        }

        if (MultiTenancySide != otherRecord.MultiTenancySide)
        {
            MultiTenancySide = otherRecord.MultiTenancySide;
        }

        if (Providers != otherRecord.Providers)
        {
            Providers = otherRecord.Providers;
        }

        if (StateCheckers != otherRecord.StateCheckers)
        {
            StateCheckers = otherRecord.StateCheckers;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            ExtraProperties.Clear();

            foreach (var property in otherRecord.ExtraProperties)
            {
                ExtraProperties.Add(property.Key, property.Value);
            }
        }
    }
}