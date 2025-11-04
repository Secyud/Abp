using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.AspNetCore.Components.Routing;
using Secyud.Abp.Identities.Localization;
using Secyud.Abp.Identities.Navigation;
using Secyud.Abp.Identities.Settings;
using Secyud.Abp.ObjectExtending;
using Secyud.Abp.Permissions;
using Secyud.Abp.Secits.Blazor;
using Secyud.Abp.Settings;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;

namespace Secyud.Abp.Identities;

[DependsOn(
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpPermissionsBlazorModule),
    typeof(AbpSettingsBlazorModule),
    typeof(AbpSecitsBlazorModule)
)]
public class AbpIdentitiesBlazorModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpIdentitiesBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpIdentitiesBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AbpIdentitiesBlazorMainMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AbpIdentitiesBlazorModule).Assembly);
        });

        Configure<SettingsComponentOptions>(options =>
        {
            options.Contributors.Add(new IdentitiesSettingsComponentContributor());
        });
        
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpIdentitiesResource>().AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.User,
                    createFormTypes: [typeof(IdentityUserCreateDto)],
                    editFormTypes: [typeof(IdentityUserUpdateDto)]
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.Role,
                    createFormTypes: [typeof(IdentityRoleCreateDto)],
                    editFormTypes: [typeof(IdentityRoleUpdateDto)]
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.ClaimType,
                    createFormTypes: [typeof(CreateClaimTypeDto)],
                    editFormTypes: [typeof(UpdateClaimTypeDto)]
                );
        });
    }
}