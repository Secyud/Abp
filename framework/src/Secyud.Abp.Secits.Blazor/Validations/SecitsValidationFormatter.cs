using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Secits.Blazor.Validations;

public class SecitsValidationFormatter(Type attributeType, Func<ValidationAttribute, string, string, string> func) : ISecitsValidationFormatter
{
    public Type AttributeType { get; } = attributeType;

    public string FormatMessage(ValidationAttribute attribute, string messageTemplate, string name)
    {
        return func(attribute, messageTemplate, name);
    }
}