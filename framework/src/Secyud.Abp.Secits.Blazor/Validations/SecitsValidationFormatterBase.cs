using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Secits.Blazor.Validations.Formatters;

public abstract class SecitsValidationFormatterBase<TAttribute> : ISecitsValidationFormatter
    where TAttribute : ValidationAttribute
{
    public Type AttributeType => typeof(TAttribute);

    public string FormatMessage(ValidationAttribute attribute, string messageTemplate, string name)
    {
        return FormatSpecificMessage((TAttribute)attribute, messageTemplate, name);
    }

    protected abstract string FormatSpecificMessage(TAttribute attribute, string messageTemplate, string name);
}