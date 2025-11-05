using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Secyud.Abp.Account.LinkUsers;
using Secyud.Abp.Account.Localization;
using Secyud.Secits.Blazor;
using Secyud.Secits.Blazor.Element;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client.Authentication;

namespace Secyud.Abp.Accounts.Pages.Account.LinkUsers;

public partial class LinkUsersModal
{
    protected SModal _modal = null!;
    protected SModal _deleteConfirmationModal = null!;
    protected SModal _newLinkUserConfirmationModal = null!;

    [Inject]
    protected IOptions<AbpAccountLinkUserOptions> Options { get; set; } = null!;

    [Inject]
    protected IIdentityLinkUserAppService LinkUserAppService { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected IAbpAccessTokenProvider AccessTokenProvider { get; set; } = null!;

    [Inject]
    protected IJSRuntime JsRuntime { get; set; } = null!;

    protected ListResultDto<LinkUserDto> LinkUsers { get; set; } = new();

    protected string? DeleteConfirmationMessage { get; set; }

    protected Guid? DeleteTenantId { get; set; }
    protected Guid DeleteUserId { get; set; }

    protected string? PostAction { get; set; }
    protected string? SourceLinkToken { get; set; }
    protected Guid? TargetLinkTenantId { get; set; }
    protected Guid TargetLinkUserId { get; set; }
    protected string? ReturnUrl { get; set; }

    private SGrid<LinkUserDto> _linkUsersDataGrid = null!;
    private string? _customFilterValue;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JsRuntime.InvokeVoidAsync("eval",
                "Array.prototype.forEach.call(document.querySelectorAll('.account_link_users_modals'), element => document.body.appendChild(element));");
        }
    }

    private Task OnCustomFilterValueChanged(string? e)
    {
        _customFilterValue = e;
        return _linkUsersDataGrid.RefreshAsync(true);
    }

    public LinkUsersModal()
    {
        LocalizationResource = typeof(AbpAccountsResource);
    }

    protected virtual async Task OpenModalAsync()
    {
        LinkUsers = await LinkUserAppService.GetAllListAsync();
        await _modal.ShowAsync();
    }


    protected virtual async Task CloseSpecifyModalAsync(SModal modal)
    {
        await modal.HideAsync();
    }

    protected virtual async Task OpenDeleteConfirmationModalAsync(Guid? tenantId, Guid userId, string userName)
    {
        DeleteTenantId = tenantId;
        DeleteUserId = userId;
        DeleteConfirmationMessage = L["DeleteLinkAccountConfirmationMessage", userName];
        await _deleteConfirmationModal.ShowAsync();
    }

    protected virtual async Task NewLinkAccountAsync(bool isConfirmed)
    {
        if (!isConfirmed)
        {
            await _newLinkUserConfirmationModal.ShowAsync();
            return;
        }

        var linkToken = await LinkUserAppService.GenerateLinkTokenAsync();

        var loginUrl = Options.Value.LoginUrl?.EnsureEndsWith('/') ?? "/";
        var returnUrl = NavigationManager.BaseUri.EnsureEndsWith('/') + "Account/Challenge";
        if (loginUrl == "/" || loginUrl == NavigationManager.BaseUri.EnsureEndsWith('/'))
        {
            returnUrl = NavigationManager.BaseUri.EnsureEndsWith('/');
        }


        var url =
            loginUrl +
            "Account/Login?handler=CreateLinkUser&" +
            "LinkUserId=" +
            CurrentUser.Id +
            "&LinkToken=" +
            UrlEncoder.Default.Encode(linkToken) +
            "&ReturnUrl=" + returnUrl;

        if (CurrentTenant.Id != null)
        {
            url += "&LinkTenantId=" + CurrentTenant.Id;
        }

        NavigationManager.NavigateTo(url, true);
    }

    protected virtual async Task LoginAsThisAccountAsync(Guid? tenantId, Guid userId)
    {
        PostAction = "/Account/LinkLogin";
        ReturnUrl = NavigationManager.Uri;
        if (!Options.Value.LoginUrl.IsNullOrEmpty())
        {
            var accessToken = await AccessTokenProvider.GetTokenAsync();
            if (!string.IsNullOrEmpty(accessToken))
            {
                PostAction = Options.Value.LoginUrl.EnsureEndsWith('/') + "Account/LinkLogin";
                PostAction += "?access_token=" + accessToken;
                ReturnUrl = NavigationManager.BaseUri.EnsureEndsWith('/') + "Account/Challenge";
            }
        }

        TargetLinkTenantId = tenantId;
        TargetLinkUserId = userId;
        SourceLinkToken = await LinkUserAppService.GenerateLinkLoginTokenAsync();

        await InvokeAsync(StateHasChanged);

        await JsRuntime.InvokeVoidAsync("eval", "document.getElementById('linkUserLoginForm').submit()");
    }

    protected virtual async Task DeleteUsersAsync()
    {
        await _deleteConfirmationModal.HideAsync();

        await LinkUserAppService.UnlinkAsync(new UnLinkUserInput
        {
            TenantId = DeleteTenantId,
            UserId = DeleteUserId
        });

        DeleteTenantId = null;
        DeleteUserId = Guid.Empty;

        DeleteConfirmationMessage = string.Empty;

        LinkUsers = await LinkUserAppService.GetAllListAsync();
        await InvokeAsync(StateHasChanged);
    }
}