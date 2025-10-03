using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Validation.StringValues;

namespace Secyud.Abp.Features.JsonConverters;

public class StringValueTypeJsonConverter(ValueValidatorFactoryOptions options) : JsonConverter<IStringValueType>
{
    private JsonSerializerOptions? _readJsonSerializerOptions;

    private JsonSerializerOptions? _writeJsonSerializerOptions;

    protected readonly ValueValidatorFactoryOptions Options = options;

    public override IStringValueType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var rootElement = JsonDocument.ParseValue(ref reader).RootElement;

        var nameJsonProperty = rootElement.EnumerateObject()
            .FirstOrDefault(x => x.Name.Equals(nameof(IStringValueType.Name), StringComparison.OrdinalIgnoreCase));
        if (nameJsonProperty.Value.ValueKind != JsonValueKind.String)
            throw new JsonException($"Can't to get the {nameof(IStringValueType.Name)} property of {nameof(IStringValueType)}!");

        var name = nameJsonProperty.Value.GetString();

        _readJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, this, new ValueValidatorJsonConverter(Options),
            new SelectionStringValueItemSourceJsonConverter());

        return name switch
        {
            "SelectionStringValueType" => rootElement.Deserialize<SelectionStringValueType>(_readJsonSerializerOptions),
            "FreeTextStringValueType" => rootElement.Deserialize<FreeTextStringValueType>(_readJsonSerializerOptions),
            "ToggleStringValueType" => rootElement.Deserialize<ToggleStringValueType>(_readJsonSerializerOptions),
            _ => throw new ArgumentException($"{nameof(IStringValueType)} named {name} was not found!")
        };
    }

    public override void Write(Utf8JsonWriter writer, IStringValueType value, JsonSerializerOptions options)
    {
        _writeJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, this);
        JsonSerializer.Serialize(writer, value, value.GetType(), _writeJsonSerializerOptions);
    }
}