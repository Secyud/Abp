using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Secyud.Abp.Identities.ExternalLoginProviders.Ldap;
using Secyud.Abp.Identities.ExternalLoginProviders.OAuth;
using Secyud.Abp.Identities.Localization;
using Secyud.Abp.Identities.Session;
using Secyud.Abp.ObjectExtending;
using Secyud.Abp.Users;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Gdpr;
using Volo.Abp.Ldap;
using Volo.Abp.Ldap.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Threading;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities;

[DependsOn(
    typeof(AbpIdentitiesDomainSharedModule),
    typeof(AbpLdapModule),
    typeof(AbpCachingModule),
    typeof(AbpDddDomainModule),
    typeof(AbpUsersDomainModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpGdprAbstractionsModule),
    typeof(AbpAspNetCoreAbstractionsModule)
)]
public class AbpIdentitiesDomainModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new();


    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IdentityBuilder>(builder => { builder.AddUserValidator<MaxUserCountValidator>(); });
        PreConfigure<AbpClaimsPrincipalFactoryOptions>(options => { options.IsRemoteRefreshEnabled = false; });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var identityBuilder = context.Services.AddAbpIdentity(options
            =>
        {
            options.User.RequireUniqueEmail = true;
        });

        context.Services.AddObjectAccessor(identityBuilder);
        context.Services.ExecutePreConfiguredActions(identityBuilder);

        Configure<IdentityOptions>(options =>
        {
            options.ClaimsIdentity.UserIdClaimType = AbpClaimTypes.UserId;
            options.ClaimsIdentity.UserNameClaimType = AbpClaimTypes.UserName;
            options.ClaimsIdentity.RoleClaimType = AbpClaimTypes.Role;
            options.ClaimsIdentity.EmailClaimType = AbpClaimTypes.Email;
        });

        context.Services.AddAbpDynamicOptions<IdentityOptions, AbpIdentityOptionsManager>();
        
        context.Services.AddScoped(typeof(IUserStore<IdentityUser>), provider => provider.GetService(typeof(IdentitiesUserStore))!);
        context.Services.AddIdentityCore<IdentityUser>().AddTokenProvider<AbpAuthenticatorTokenProvider>(TokenOptions.DefaultAuthenticatorProvider);

        Configure<AbpIdentityOptions>(options =>
        {
            options.ExternalLoginProviders.Add<LdapExternalLoginProvider>(LdapExternalLoginProvider.Name);
            options.ExternalLoginProviders.Add<OAuthExternalLoginProvider>(OAuthExternalLoginProvider.Name);
        });

        context.Services.AddHttpClient(OAuthExternalLoginManager.HttpClientName);

        context.Services.AddAutoMapperObjectMapper<AbpIdentitiesDomainModule>();

        Configure<AbpAutoMapperOptions>(options => { options.AddProfile<IdentityDomainMappingProfile>(validate: true); });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<IdentityUser, UserEto>(typeof(AbpIdentitiesDomainModule));
            options.EtoMappings.Add<IdentityClaimType, IdentityClaimTypeEto>(typeof(AbpIdentitiesDomainModule));
            options.EtoMappings.Add<IdentityRole, IdentityRoleEto>(typeof(AbpIdentitiesDomainModule));

            options.AutoEventSelectors.Add<IdentityUser>();
            options.AutoEventSelectors.Add<IdentityRole>();
        });
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpIdentitiesResource>()
                .AddBaseTypes(typeof(LdapResource));
        });

    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.User,
                typeof(IdentityUser)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.Role,
                typeof(IdentityRole)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.ClaimType,
                typeof(IdentityClaimType)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.IdentitySession,
                typeof(IdentitySession)
            );
        });
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<IdentitySessionCleanupOptions>>().Value;
        if (options.IsCleanupEnabled)
        {
            await context.ServiceProvider
                .GetRequiredService<IBackgroundWorkerManager>()
                .AddAsync(context.ServiceProvider.GetRequiredService<IdentitySessionCleanupBackgroundWorker>());
        }
    }
}