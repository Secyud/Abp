using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Secyud.Abp.Identities.Localization;
using Secyud.Abp.Identities.Navigation;

namespace Secyud.Abp.Identities.ClaimTypes;

[Authorize(IdentityPermissions.Users.DefaultName)]
[Route(IdentitiesMenus.ClaimTypesUrl)]
public partial class Index
{
    public Index()
    {
        ObjectMapperContext = typeof(AbpIdentitiesBlazorModule);
        LocalizationResource = typeof(AbpIdentitiesResource);

        CreatePolicyName = IdentityPermissions.ClaimTypes.Create.Name;
        UpdatePolicyName = IdentityPermissions.ClaimTypes.Update.Name;
        DeletePolicyName = IdentityPermissions.ClaimTypes.Delete.Name;
    }

    protected override string GetDeleteConfirmationMessage(ClaimTypeDto entity)
    {
        return string.Format(L["ClaimTypeDeletionConfirmationMessage"], entity.Name);
    }
}