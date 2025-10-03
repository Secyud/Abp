using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Json.SystemTextJson.JsonConverters;
using Volo.Abp.Validation.StringValues;

namespace Secyud.Abp.Features.JsonConverters;

public class ValueValidatorJsonConverter(ValueValidatorFactoryOptions options) : JsonConverter<IValueValidator>
{
    private JsonSerializerOptions? _readJsonSerializerOptions;

    private JsonSerializerOptions? _writeJsonSerializerOptions;

    protected readonly ValueValidatorFactoryOptions Options = options;

    public override IValueValidator Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var rootElement = JsonDocument.ParseValue(ref reader).RootElement;

        var nameJsonProperty = rootElement.EnumerateObject()
            .FirstOrDefault(x => x.Name.Equals(nameof(IValueValidator.Name), StringComparison.OrdinalIgnoreCase));
        if (nameJsonProperty.Value.ValueKind != JsonValueKind.String)
            throw new JsonException($"Can't to get the {nameof(IValueValidator.Name)} property of {nameof(IValueValidator)}!");
        var valueValidator = CreateValueValidatorByName(nameJsonProperty.Value.GetString());

        var propertiesJsonProperty = rootElement.EnumerateObject()
            .FirstOrDefault(x => x.Name.Equals(nameof(IValueValidator.Properties), StringComparison.OrdinalIgnoreCase));
        if (propertiesJsonProperty.Value.ValueKind == JsonValueKind.Object)
        {
            _readJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, this, new ObjectToInferredTypesConverter());
            var properties = propertiesJsonProperty.Value.Deserialize<IDictionary<string, object>>(_readJsonSerializerOptions);
            if (properties != null && properties.Any())
            {
                foreach (var property in properties)
                {
                    valueValidator.Properties[property.Key] = property.Value;
                }
            }
        }

        return valueValidator;
    }

    public override void Write(Utf8JsonWriter writer, IValueValidator value, JsonSerializerOptions options)
    {
        _writeJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, this);
        JsonSerializer.Serialize(writer, value, value.GetType(), _writeJsonSerializerOptions);
    }

    protected virtual IValueValidator CreateValueValidatorByName(string? name)
    {
        foreach (var factory in Options.ValueValidatorFactory.Where(factory => factory.CanCreate(name)))
        {
            return factory.Create();
        }

        throw new ArgumentException($"{nameof(IValueValidator)} named {name} was cannot be created!");
    }
}