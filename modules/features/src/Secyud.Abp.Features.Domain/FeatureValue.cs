using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Secyud.Abp.Features;

public class FeatureValue : Entity<Guid>, IAggregateRoot<Guid>
{
    public string Name { get; protected set; } = string.Empty;

    public string? Value { get; internal set; }

    public string? ProviderName { get; protected set; }

    public string? ProviderKey { get; protected set; }

    protected FeatureValue()
    {
    }

    public FeatureValue(
        Guid id,
        string name,
        string? value,
        string? providerName,
        string? providerKey)
        : base(id)
    {
        Check.NotNull(name, nameof(name));

        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        Value = Check.NotNullOrWhiteSpace(value, nameof(value));
        ProviderName = Check.NotNullOrWhiteSpace(providerName, nameof(providerName));
        ProviderKey = providerKey;
    }
}