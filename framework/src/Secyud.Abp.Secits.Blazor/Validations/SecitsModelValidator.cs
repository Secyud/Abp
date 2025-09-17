using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Secyud.Secits.Blazor.Validations;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Secits.Blazor.Validations;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(DefaultSecitsModelValidator))]
public class SecitsModelValidator(ISecitsValidationFormatterProvider formatterProvider, IServiceProvider serviceProvider)
    : DefaultSecitsModelValidator, ITransientDependency
{
    private Dictionary<Type, IStringLocalizer> Localizers { get; } = [];

    public override async Task<List<ValidationResult>> ValidateValueAsync(ValidationContext context, object? value)
    {
        Check.NotNullOrEmpty(context.MemberName, nameof(context.MemberName));
        string? displayName = null;
        bool displayNameSet = false;
        var memberInfo = context.ObjectType.GetMember(context.MemberName)[0];
        var attributes = memberInfo.GetCustomAttributes(false);
        var validationAttributes = attributes.OfType<ValidationAttribute>();
        var validationResults = new List<ValidationResult>();
        foreach (var attribute in validationAttributes)
        {
            if (attribute.IsValid(value)) continue;

            var messageTemplate = GetErrorMessageString(attribute);
            if (attribute.ErrorMessageResourceType is not null)
            {
                messageTemplate = L(attribute.ErrorMessageResourceType, messageTemplate);
            }

            var messageFormater = await formatterProvider.GetValidationFormatterAsync(attribute.GetType());
            var message = messageFormater.FormatMessage(attribute, messageTemplate, GetDisplayName());
            var result = new ValidationResult(message, [context.MemberName]);
            validationResults.Add(result);
        }


        return validationResults;

        string GetDisplayName()
        {
            if (!displayNameSet)
            {
                var displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();
                if (displayAttribute is not null)
                {
                    if (displayAttribute.ResourceType is null)
                    {
                        displayName = displayAttribute.Name;
                    }
                    else if (displayAttribute.Name is not null)
                    {
                        displayName = L(displayAttribute.ResourceType, displayAttribute.Name);
                    }
                }

                displayNameSet = true;
            }

            return displayName ?? context.MemberName;
        }
    }

    private string L(Type resourceType, string key)
    {
        if (!Localizers.TryGetValue(resourceType, out var l))
        {
            var localizerType = typeof(IStringLocalizer<>).MakeGenericType(resourceType);
            l = (IStringLocalizer)serviceProvider.GetRequiredService(localizerType);
            Localizers[resourceType] = l;
        }

        return l[key];
    }

    private static string GetErrorMessageString(ValidationAttribute attribute)
    {
        var type = attribute.GetType();
        var getter = type.GetProperty("ErrorMessageString",
            BindingFlags.Instance | BindingFlags.NonPublic);
        return getter?.GetValue(attribute)?.ToString()!;
    }
}