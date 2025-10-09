using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Validation.StringValues;

namespace Secyud.Abp.Features;

public class StringValueTypeSerializer(IJsonSerializer jsonSerializer) : ITransientDependency
{
    protected IJsonSerializer JsonSerializer { get; } = jsonSerializer;

    public virtual string Serialize(IStringValueType stringValueType)
    {
        return JsonSerializer.Serialize(stringValueType);
    }

    public virtual IStringValueType Deserialize(string value)
    {
        return JsonSerializer.Deserialize<IStringValueType>(value);
    }
}
