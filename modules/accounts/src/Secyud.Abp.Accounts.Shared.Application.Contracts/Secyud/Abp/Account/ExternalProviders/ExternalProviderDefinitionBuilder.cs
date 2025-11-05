using System.Linq.Expressions;
using System.Reflection;
using Volo.Abp;

namespace Secyud.Abp.Account.ExternalProviders;

public class ExternalProviderDefinitionBuilder<TOptions>
{
    public string AuthenticationSchema { get; }

    public List<ExternalProviderDefinitionProperty> Properties { get; }

    internal ExternalProviderDefinitionBuilder(string authenticationSchema)
    {
        AuthenticationSchema = authenticationSchema;
        Properties = new List<ExternalProviderDefinitionProperty>();
    }

    public ExternalProviderDefinitionBuilder<TOptions> WithProperty<TProperty>(
        Expression<Func<TOptions, TProperty>> propertySelector,
        bool isSecret = false)
    {
        Check.NotNull(propertySelector, nameof(propertySelector));

        var memberExpression = propertySelector.Body as MemberExpression;
        if (memberExpression == null)
        {
            throw new ArgumentException($"{nameof(propertySelector)} must be a property expression!");
        }

        var propertyInfo = memberExpression.Member as PropertyInfo;
        if (propertyInfo == null)
        {
            throw new ArgumentException($"{nameof(propertySelector)} must be a property expression!");
        }

        Properties.Add(new ExternalProviderDefinitionProperty
        {
            PropertyName = propertyInfo.Name,
            IsSecret = isSecret
        });

        return this;
    }

    internal ExternalProviderDefinition Build()
    {
        return new ExternalProviderDefinition
        {
            Name = AuthenticationSchema,
            Properties = Properties
        };
    }
}
