using Volo.Abp;

namespace Secyud.Abp.Identities;

public class ExternalLoginProviderInfo(string name, Type type)
{
    public string Name { get; } = Check.NotNullOrWhiteSpace(name, nameof(name));

    public Type Type {
        get => _type;
        set => _type = Check.NotNull(value, nameof(value));
    }
    private Type _type = Check.AssignableTo<IExternalLoginProvider>(type, nameof(type));
}
