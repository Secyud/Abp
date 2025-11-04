using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Identities.AspNetCore;

[DependsOn(
    typeof(AbpIdentitiesDomainModule)
    )]
public class AbpIdentityAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IdentityBuilder>(builder =>
        {
            builder
                .AddDefaultTokenProviders()
                .AddTokenProvider<LinkUserTokenProvider>(LinkUserTokenProviderConsts.LinkUserTokenProviderName)
                .AddSignInManager<AbpSignInManager>()
                .AddUserValidator<AbpIdentityUserValidator>();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //(TODO: Extract an extension method like IdentityBuilder.AddAbpSecurityStampValidator())
        context.Services.AddScoped<AbpSecurityStampValidator>();
        context.Services.AddScoped(typeof(SecurityStampValidator<IdentityUser>), provider => provider.GetService(typeof(AbpSecurityStampValidator))!);
        context.Services.AddScoped(typeof(ISecurityStampValidator), provider => provider.GetService(typeof(AbpSecurityStampValidator))!);

        var options = context.Services.ExecutePreConfiguredActions(new AbpIdentityAspNetCoreOptions());

        if (options.ConfigureAuthentication)
        {
            context.Services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();
        }
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddOptions<SecurityStampValidatorOptions>()
            .Configure<IServiceProvider>((securityStampValidatorOptions, serviceProvider) =>
            {
                var abpRefreshingPrincipalOptions = serviceProvider.GetRequiredService<IOptions<AbpRefreshingPrincipalOptions>>().Value;
                securityStampValidatorOptions.UpdatePrincipal(abpRefreshingPrincipalOptions);
            });
    }
}
