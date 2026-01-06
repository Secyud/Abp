using System.ComponentModel.DataAnnotations;
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

    public PermissionDefinitionRecord(Guid id) : base(id)
    {
        this.SetDefaultsForExtraProperties();
    }

    public required string Name { get; set; }

    public required string DisplayName { get; set; }

    public string? ParentName { get; set; }

    public MultiTenancySides MultiTenancySide { get; set; }
    public string? Providers { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; protected set; } = [];

    public bool HasSameData(PermissionDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
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

        if (MultiTenancySide != otherRecord.MultiTenancySide)
        {
            return false;
        }

        if (Providers != otherRecord.Providers)
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

        if (ParentName != otherRecord.ParentName)
        {
            ParentName = otherRecord.ParentName;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            DisplayName = otherRecord.DisplayName;
        }

        if (MultiTenancySide != otherRecord.MultiTenancySide)
        {
            MultiTenancySide = otherRecord.MultiTenancySide;
        }

        if (Providers != otherRecord.Providers)
        {
            Providers = otherRecord.Providers;
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