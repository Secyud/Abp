using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Secyud.Abp.Features.Components;
using Secyud.Abp.Tenants.Localization;
using Secyud.Abp.Tenants.Navigation;

namespace Secyud.Abp.Tenants;

[Authorize(TenantsPermissions.Tenants.DefaultName)]
[Route(TenantsMenuNames.TenantsUri)]
public partial class Index
{
    protected const string FeatureProviderName = "T";

    protected bool HasManageFeaturesPermission;
    protected string ManageFeaturesPolicyName;

    protected FeaturesModal? FeaturesModal;

    protected bool ShowPassword { get; set; }

    public Index()
    {
        LocalizationResource = typeof(AbpTenantsResource);
        ObjectMapperContext = typeof(AbpTenantsBlazorModule);

        CreatePolicyName = TenantsPermissions.Tenants.Create.Name;
        UpdatePolicyName = TenantsPermissions.Tenants.Update.Name;
        DeletePolicyName = TenantsPermissions.Tenants.Delete.Name;

        ManageFeaturesPolicyName = TenantsPermissions.Tenants.ManageFeatures.Name;
    }

    protected override async Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();

        HasManageFeaturesPermission = await AuthorizationService.IsGrantedAsync(ManageFeaturesPolicyName);
    }

    protected override string GetDeleteConfirmationMessage(TenantDto entity)
    {
        return string.Format(L["TenantDeletionConfirmationMessage"], entity.Name);
    }


    protected virtual void TogglePasswordVisibility()
    {
        ShowPassword = !ShowPassword;
    }
}