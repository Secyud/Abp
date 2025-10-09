using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Secyud.Abp.Settings;

public class Setting : Entity<Guid>, IAggregateRoot<Guid>
{
    public string Name { get; protected set; } = string.Empty;

    public string Value { get; internal set; } = string.Empty;

    public string? ProviderName { get; protected set; }

    public string? ProviderKey { get; protected set; }

    protected Setting()
    {
    }

    public Setting(
        Guid id,
        string name,
        string value,
        string? providerName,
        string? providerKey = null)
        : base(id)
    {
        Check.NotNull(name, nameof(name));
        Check.NotNull(value, nameof(value));

        Name = name;
        Value = value;
        ProviderName = providerName;
        ProviderKey = providerKey;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, Name = {Name}, Value = {Value}, ProviderName = {ProviderName}, ProviderKey = {ProviderKey}";
    }
}