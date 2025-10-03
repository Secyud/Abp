using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Secyud.Abp.AspNetCore.Components.Toolbars;
using Secyud.Abp.Features.Components;
using Secyud.Abp.ObjectExtending;
using Secyud.Abp.Tenants.Localization;
using Secyud.Abp.Tenants.Navigation;
using Secyud.Secits.Blazor.Icons;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;

namespace Secyud.Abp.Tenants.Components;

[Route(TenantsMenuNames.TenantsUri)]
public partial class Tenants
{
    protected const string FeatureProviderName = "T";

    protected bool HasManageFeaturesPermission;
    protected string ManageFeaturesPolicyName;

    protected FeaturesModal? FeaturesModal;

    protected bool ShowPassword { get; set; }

    public Tenants()
    {
        LocalizationResource = typeof(AbpTenantsResource);
        ObjectMapperContext = typeof(AbpTenantsBlazorModule);

        CreatePolicyName = TenantsPermissions.Tenants.Create;
        UpdatePolicyName = TenantsPermissions.Tenants.Update;
        DeletePolicyName = TenantsPermissions.Tenants.Delete;

        ManageFeaturesPolicyName = TenantsPermissions.Tenants.ManageFeatures;
    }

    [Inject]
    public IIconProvider IconProvider { get; set; } = null!;

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