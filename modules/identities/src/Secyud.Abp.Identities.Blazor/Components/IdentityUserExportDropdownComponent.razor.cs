using System.Globalization;
using Microsoft.AspNetCore.Components;
using Secyud.Abp.Identities.Users;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Json;

namespace Secyud.Abp.Identities.Components;

public partial class IdentityUserExportDropdownComponent
{
    protected const string ExportToExcelEndpoint = "api/identities/users/export-as-excel";
    protected const string ExportToCsvEndpoint = "api/identities/users/export-as-csv";

    [Inject]
    protected IIdentityUserAppService IdentityUserAppService { get; set; } = null!;

    [Inject]
    protected IServerUrlProvider ServerUrlProvider { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected IJsonSerializer JsonSerializer { get; set; } = null!;

    [Inject]
    protected UserManagementState UserManagementState { get; set; } = null!;

    protected virtual async Task ExportToExcelFileAsync()
    {
        await ExportToFileAsync(ExportToExcelEndpoint);
    }

    protected virtual async Task ExportToCsvFileAsync()
    {
        await ExportToFileAsync(ExportToCsvEndpoint);
    }

    private async Task ExportToFileAsync(string endpoint)
    {
        var filterJsonStr = JsonSerializer.Serialize(UserManagementState.GetFilter());
        var queryString = JsonSerializer.Deserialize<Dictionary<string, object>>(filterJsonStr).Select(x => x.Key + "=" + x.Value).JoinAsString("&");

        var baseUrl = await ServerUrlProvider.GetBaseUrlAsync(IdentitiesRemoteServiceConsts.RemoteServiceName);
        var downloadToken = await IdentityUserAppService.GetDownloadTokenAsync();
        var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
        if (!culture.IsNullOrEmpty())
        {
            culture = "&culture=" + culture;
        }

        NavigationManager.NavigateTo($"{baseUrl}{endpoint}?token={downloadToken.Token}&{queryString}{culture}", forceLoad: true);
    }
}