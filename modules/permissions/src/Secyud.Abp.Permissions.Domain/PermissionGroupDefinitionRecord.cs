using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Secyud.Abp.Permissions;

public class PermissionGroupDefinitionRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
    public PermissionGroupDefinitionRecord()
    {
        this.SetDefaultsForExtraProperties();
    }

    public PermissionGroupDefinitionRecord(Guid id,
        string name,
        string? displayName) : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), PermissionGroupDefinitionRecordConsts.MaxNameLength);
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), PermissionGroupDefinitionRecordConsts.MaxDisplayNameLength);
        this.SetDefaultsForExtraProperties();
    }

    public string Name { get; set; } = "";

    public string? DisplayName { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; protected set; } = [];

    public bool HasSameData(PermissionGroupDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            return false;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            return false;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            return false;
        }

        return true;
    }

    public void Patch(PermissionGroupDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            Name = otherRecord.Name;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            DisplayName = otherRecord.DisplayName;
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