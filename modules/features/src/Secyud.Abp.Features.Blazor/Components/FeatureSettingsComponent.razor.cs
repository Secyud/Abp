using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Secyud.Abp.Features.Localization;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;

namespace Secyud.Abp.Features.Components;

public partial class FeatureSettingsComponent : AbpComponentBase
{
    [Inject]
    protected PermissionChecker PermissionChecker { get; set; } = null!;

    protected FeaturesModal? FeaturesModal { get; set; }

    protected FeatureSettingViewModel? Settings;

    public FeatureSettingsComponent()
    {
        LocalizationResource = typeof(AbpFeaturesResource);
    }

    protected override async Task OnInitializedAsync()
    {
        Settings = new FeatureSettingViewModel
        {
            HasManageHostFeaturesPermission = await AuthorizationService.IsGrantedAsync(FeaturesPermissions.ManageHostFeatures)
        };
    }

    protected virtual async Task OnManageHostFeaturesClicked()
    {
        await FeaturesModal!.OpenAsync(TenantFeatureValueProvider.ProviderName);
    }
}