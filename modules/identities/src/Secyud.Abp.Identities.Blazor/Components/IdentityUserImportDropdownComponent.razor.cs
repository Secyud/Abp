using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MimeTypes;
using Secyud.Abp.Identities.Localization;
using Secyud.Abp.Identities.Users;
using Secyud.Secits.Blazor.Element;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Content;

namespace Secyud.Abp.Identities.Components;

public partial class IdentityUserImportDropdownComponent : AbpComponentBase
{
    public const string ImportInvalidUsersEndpoint = "api/identities/users/download-import-invalid-users-file";
    public const string DownloadImportSampleFileEndpoint = "api/identities/users/import-users-sample-file";

    protected SModal ExternalUserModal = null!;
    protected SModal UploadFileModal = null!;

    protected ExternalUserViewModel ExternalUser = new();
    protected UploadFileViewModel UploadFile = new();

    protected List<ExternalLoginProviderDto> ExternalLoginProviders = [];

    [Inject]
    protected IIdentityUserAppService IdentityUserAppService { get; set; } = null!;

    [Inject]
    protected IServerUrlProvider ServerUrlProvider { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected UserManagementState UserManagementState { get; set; } = null!;


    public IdentityUserImportDropdownComponent()
    {
        LocalizationResource = typeof(AbpIdentitiesResource);
    }

    protected override async Task OnInitializedAsync()
    {
        ExternalLoginProviders = await IdentityUserAppService.GetExternalLoginProvidersAsync();
        ExternalUser.Provider = ExternalLoginProviders.FirstOrDefault()?.Name ?? "";
    }

    protected virtual Task OpenExternalUserModalAsync()
    {
        if (!ExternalLoginProviders.Any())
        {
            return Message.Warn(L["NoExternalLoginProviderAvailable"]);
        }

        return ExternalUserModal.ShowAsync();
    }

    protected virtual Task CloseExternalUserModal()
    {
        ExternalUser = new ExternalUserViewModel { Provider = ExternalLoginProviders.First().Name };
        return ExternalUserModal.HideAsync();
    }

    protected virtual async Task CloseUploadFileModal()
    {
        await UserManagementState.DataGridChangedAsync();
        await UploadFileModal.HideAsync();
    }

    protected virtual async Task ImportExternalUserAsync()
    {
        try
        {
            if (ExternalUserModal.Validate())
            {
                await IdentityUserAppService.ImportExternalUserAsync(new ImportExternalUserInput
                {
                    Provider = ExternalUser.Provider,
                    UserNameOrEmailAddress = ExternalUser.UserNameOrEmailAddress,
                    Password = ExternalUser.Password,
                });


                await CloseExternalUserModal();
                await UserManagementState.DataGridChangedAsync();
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task OpenUploadFileModalAsync(ImportUsersFromFileType fileType)
    {
        UploadFile = new UploadFileViewModel { FileType = fileType, FileName = L["ChooseFile"] };
        return UploadFileModal.ShowAsync();
    }

    protected virtual async Task OnChooseFileChangedAsync(InputFileChangeEventArgs e)
    {
        try
        {
            var file = e.File;
            UploadFile.FileName = file.Name;
            UploadFile.File = file;
        }
        catch (Exception ex)
        {
            UploadFile.FileName = L["ChooseFile"];
            UploadFile.File = null;
            await HandleErrorAsync(ex);
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    protected virtual async Task ImportUsersFromFileAsync()
    {
        try
        {
            if (UploadFile.File == null)
            {
                await Message.Warn(L["PleaseSelectAFile"]);
                return;
            }

            var result = await IdentityUserAppService.ImportUsersFromFileAsync(new ImportUsersFromFileInputWithStream
            {
                FileType = UploadFile.FileType,
                File = new RemoteStreamContent(UploadFile.File.OpenReadStream(), UploadFile.FileName,
                    MimeTypeMap.GetMimeType(UploadFile.FileName.Split('.').Last().ToLower()))
            });

            if (result.IsAllSucceeded)
            {
                await Message.Success(L["ImportSuccessMessage"]);
                await CloseUploadFileModal();
            }
            else
            {
                if (await Message.Confirm(L["ImportFailedMessage", result.SucceededCount, result.FailedCount]))
                {
                    var baseUrl = await ServerUrlProvider.GetBaseUrlAsync(IdentitiesRemoteServiceConsts.RemoteServiceName);
                    NavigationManager.NavigateTo($"{baseUrl}{ImportInvalidUsersEndpoint}/?token={result.InvalidUsersDownloadToken}", forceLoad: true);
                }
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task DownloadImportSampleFileAsync()
    {
        try
        {
            var downloadToken = await IdentityUserAppService.GetDownloadTokenAsync();
            var baseUrl = await ServerUrlProvider.GetBaseUrlAsync(IdentitiesRemoteServiceConsts.RemoteServiceName);
            var culture = CultureInfo.CurrentUICulture.Name;
            if (!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }

            NavigationManager.NavigateTo(
                $"{baseUrl}{DownloadImportSampleFileEndpoint}?fileType={(int)UploadFile.FileType}&token={downloadToken.Token}{culture}", forceLoad: true);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
}

public class ExternalUserViewModel
{
    [Required]
    public string Provider { get; set; } = string.Empty;

    [Required]
    public string UserNameOrEmailAddress { get; set; } = string.Empty;

    public string? Password { get; set; }
}

public class UploadFileViewModel
{
    public ImportUsersFromFileType FileType { get; set; }

    [Required]
    public IBrowserFile? File { get; set; }

    [Required]
    public string FileName { get; set; } = "";

    public string Accept
    {
        get
        {
            switch (FileType)
            {
                case ImportUsersFromFileType.Excel:
                default:
                    return ".xlsx,.xls";
                case ImportUsersFromFileType.Csv:
                    return ".csv";
            }
        }
    }
}