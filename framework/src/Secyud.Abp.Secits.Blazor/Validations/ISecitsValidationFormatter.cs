using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Secits.Blazor.Validations;

public interface ISecitsValidationFormatter
{
    Type AttributeType { get; }
    string FormatMessage(ValidationAttribute attribute, string messageTemplate, string name);
}