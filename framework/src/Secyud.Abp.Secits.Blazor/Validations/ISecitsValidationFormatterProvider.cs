using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Secits.Blazor.Validations;

public interface ISecitsValidationFormatterProvider
{
    Task<ISecitsValidationFormatter> GetValidationFormatterAsync(Type attributeType);
}