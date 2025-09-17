using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Secits.Blazor.Validations;

public class SecitsValidationFormatterProvider(IOptions<AbpSecitsValidationOptions> options) : ISecitsValidationFormatterProvider, ISingletonDependency
{
    protected AbpSecitsValidationOptions Options { get; } = options.Value;

    protected Dictionary<Type, ISecitsValidationFormatter> Formatters { get; } = new();

    public virtual async Task<ISecitsValidationFormatter> GetValidationFormatterAsync(Type attributeType)
    {
        if (!Formatters.TryGetValue(attributeType, out var formatter))
        {
            if (!attributeType.IsAssignableTo(typeof(ValidationAttribute)))
                throw new InvalidOperationException($"The attribute type must be a subclass of ValidationAttribute. {attributeType}");

            foreach (var optionsFormatter in Options.Formatters)
            {
                if (optionsFormatter.AttributeType == attributeType)
                {
                    formatter = optionsFormatter;
                    break;
                }

                if (optionsFormatter.AttributeType.IsAssignableFrom(attributeType))
                {
                    formatter = optionsFormatter;
                }
            }

            formatter ??= new SecitsValidationFormatter(attributeType, (_, template, name) => string.Format(template, name));

            Formatters[attributeType] = formatter;
        }

        return await Task.FromResult(formatter);
    }
}