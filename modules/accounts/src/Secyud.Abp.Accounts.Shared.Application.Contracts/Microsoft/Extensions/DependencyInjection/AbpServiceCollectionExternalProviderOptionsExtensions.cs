using Secyud.Abp.Account.ExternalProviders;
using Volo.Abp;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpServiceCollectionExternalProviderOptionsExtensions
{
    public static IServiceCollection AddDynamicExternalLoginProviderOptions<TOptions>(
        this IServiceCollection services,
        string authenticationSchema,
        Action<ExternalProviderDefinitionBuilder<TOptions>> buildAction)
        where TOptions : class, new()
    {
        Check.NotNull(services, nameof(services));
        Check.NotNullOrWhiteSpace(authenticationSchema, nameof(authenticationSchema));
        Check.NotNull(buildAction, nameof(buildAction));

        var builder = new ExternalProviderDefinitionBuilder<TOptions>(authenticationSchema);

        buildAction(builder);

        services.Configure<AbpExternalProviderOptions>(options =>
        {
            options.Definitions.Add(builder.Build());
        });

        return services;
    }
}
