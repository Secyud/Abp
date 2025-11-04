using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Secyud.Abp.Identities;

public class IdentityClaimType : AggregateRoot<Guid>, IHasCreationTime
{
    public string Name { get; protected set; } = "";

    public bool Required { get; set; }

    public bool IsStatic { get; protected set; }

    public string? Regex { get; set; }

    public string? RegexDescription { get; set; }

    public string? Description { get; set; }

    public IdentityClaimValueType ValueType { get; set; }

    public DateTime CreationTime { get; protected set; }

    protected IdentityClaimType()
    {
    }

    public IdentityClaimType(
        Guid id,
        string name,
        bool required = false,
        bool isStatic = false,
        string? regex = null,
        string? regexDescription = null,
        string? description = null,
        IdentityClaimValueType valueType = IdentityClaimValueType.String)
        : base(id)
    {
        SetName(name);
        Required = required;
        IsStatic = isStatic;
        Regex = regex;
        RegexDescription = regexDescription;
        Description = description;
        ValueType = valueType;
    }

    public void SetName(string name)
    {
        Name = Check.NotNull(name, nameof(name));
    }
}