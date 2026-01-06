using Secyud.Abp.Authorization;
using Secyud.Abp.Ui.Navigation.Localization.Resource;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.UI;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Ui.Navigation;

[DependsOn(
    typeof(AbpUiModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpMultiTenancyModule))]
public class AbpUiNavigationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options
            =>
        {
            options.FileSets.AddEmbedded<AbpUiNavigationModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpUiNavigationResource>("en")
                .AddVirtualJson("/Secyud/Abp/Ui/Navigation/Localization/Resource");
        });

        Configure<AbpNavigationOptions>(options
            =>
        {
            options.MenuContributors.Add(new DefaultMenuContributor());
        });

        Configure<AbpNavigationOptions>(options
            =>
        {
            options.MainMenuNames.Add(StandardMenus.Main);
        });
    }
}