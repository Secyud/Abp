using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Secyud.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security;

namespace Secyud.Abp.Authorization;

[DependsOn(
    typeof(AbpAuthorizationAbstractionsModule),
    typeof(AbpSecurityModule),
    typeof(AbpMultiTenancyModule)
)]
public class AbpAuthorizationModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(AuthorizationInterceptorRegistrar.RegisterIfNeeded);
        AutoAddDefinitionProviders(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAuthorizationCore();

        context.Services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        context.Services.AddSingleton<IAuthorizationHandler, PermissionsRequirementHandler>();

        context.Services.TryAddTransient<DefaultAuthorizationPolicyProvider>();
    }

    private static void AutoAddDefinitionProviders(IServiceCollection services)
    {
        var definitionProviders = new List<Type>();

        services.OnRegistered(context =>
        {
            if (typeof(IPermissionDefinitionProvider).IsAssignableFrom(context.ImplementationType))
            {
                definitionProviders.Add(context.ImplementationType);
            }
        });

        services.Configure<AbpPermissionOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(definitionProviders);
        });
    }
}
