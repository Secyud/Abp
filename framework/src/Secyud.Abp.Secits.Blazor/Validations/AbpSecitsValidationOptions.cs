using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Secits.Blazor.Validations;

public class AbpSecitsValidationOptions
{
    public List<ISecitsValidationFormatter> Formatters { get; } = [];

    public void AddFormatter<TAttribute>(Func<TAttribute, string, string, string> formatFunc)
        where TAttribute : ValidationAttribute
    {
        Formatters.Add(new SecitsValidationFormatter(typeof(TAttribute), Format));
        return;

        string Format(ValidationAttribute attribute, string template, string name)
        {
            return formatFunc((TAttribute)attribute, template, name);
        }
    }
}