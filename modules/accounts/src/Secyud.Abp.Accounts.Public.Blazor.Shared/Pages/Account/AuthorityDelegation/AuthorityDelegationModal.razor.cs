using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Secyud.Abp.Account.AuthorityDelegation;
using Secyud.Abp.Account.Localization;
using Secyud.Secits.Blazor;
using Secyud.Secits.Blazor.Element;
using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Accounts.Pages.Account.AuthorityDelegation;

public partial class AuthorityDelegationModal
{
    protected SModal _modal = null!;

    protected SModal _delegateNewUserModal = null!;

    protected SModal _deleteConfirmationModal = null!;

    [Inject]
    protected IOptions<AbpAccountAuthorityDelegationOptions> Options { get; set; } = null!;

    [Inject]
    protected IIdentityUserDelegationAppService UserDelegationAppService { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected IJSRuntime JsRuntime { get; set; } = null!;

    protected ListResultDto<UserDelegationDto> DelegatedUsers { get; set; } = new();

    protected ListResultDto<UserDelegationDto> MyDelegatedUsers { get; set; } = new();

    protected ListResultDto<UserLookupDto> Users { get; set; } = new();

    protected string? DeleteConfirmationMessage { get; set; }

    protected Guid DeleteId { get; set; }

    protected DelegateNewUserInput DelegateNewUserInput { get; set; } = new();

    protected IReadOnlyList<DateTime?> DelegateNewUserDataRange = [];


    public AuthorityDelegationModal()
    {
        LocalizationResource = typeof(AbpAccountsResource);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JsRuntime.InvokeVoidAsync("eval",
                "Array.prototype.forEach.call(document.querySelectorAll('.account_authority_delegation_modals'), element => document.body.appendChild(element));");
        }
    }

    protected virtual async Task GetDelegatedUsersAsync()
    {
        DelegatedUsers = await UserDelegationAppService.GetDelegatedUsersAsync();
    }


    protected virtual async Task GetMyDelegatedUsersAsync()
    {
        MyDelegatedUsers = await UserDelegationAppService.GetMyDelegatedUsersAsync();
    }

    protected virtual async Task OpenDeleteConfirmationModalAsync(Guid id, string userName)
    {
        DeleteId = id;
        DeleteConfirmationMessage = L["DeleteUserDelegationConfirmationMessage", userName];
        await _deleteConfirmationModal.ShowAsync();
    }

    protected virtual async Task DeleteDelegatedUsersAsync()
    {
        await CloseConfirmationModal();
        await UserDelegationAppService.DeleteDelegationAsync(DeleteId);
        await GetDelegatedUsersAsync();
        DeleteId = Guid.Empty;
        DeleteConfirmationMessage = string.Empty;
    }

    protected virtual async Task DelegateNewUserAsync()
    {
        try
        {
            if (DelegateNewUserInput.TargetUserId == Guid.Empty)
            {
                await Message.Error(@L["AuthorityDelegation:PleaseSelectUser"]);
                return;
            }

            if (DelegateNewUserDataRange.Min() == null || DelegateNewUserDataRange.Max() == null)
            {
                await Message.Error(@L["AuthorityDelegation:PleaseSelectDelegationDateRange"]);
                return;
            }

            DelegateNewUserInput.StartTime = DelegateNewUserDataRange.Min();
            DelegateNewUserInput.EndTime = DelegateNewUserDataRange.Max();
            await UserDelegationAppService.DelegateNewUserAsync(DelegateNewUserInput);
            await GetDelegatedUsersAsync();
            await CloseDelegateNewUserModal();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task GetUserLookupAsync(string searchValue)
    {
        Users = await UserDelegationAppService.GetUserLookupAsync(
            new GetUserLookupInput
            {
                UserName = searchValue
            });
    }

    protected virtual async Task OpenModalAsync()
    {
        await GetDelegatedUsersAsync();
        await GetMyDelegatedUsersAsync();
        await _modal.ShowAsync();
    }

    protected virtual async Task OpenDelegateNewUserModalAsync()
    {
        DelegateNewUserInput = new DelegateNewUserInput();
        DelegateNewUserDataRange = new List<DateTime?> { null, null };
        await _delegateNewUserModal.ShowAsync();
    }

    protected virtual string GetStatus(UserDelegationDto content)
    {
        var status = "";
        var curr = DateTime.Now;
        if (content.StartTime > curr)
        {
            status = "Future";
        }
        else if (curr > content.EndTime)
        {
            status = "Expired";
        }
        else if (content.StartTime < curr && curr < content.EndTime)
        {
            status = "Active";
        }

        return status;
    }

    protected virtual string GetStatusBadge(string status)
    {
        var badge = status switch
        {
            "Future" => "warning",
            "Expired" => "danger",
            "Active" => "success",
            _ => ""
        };
        return badge;
    }

    protected bool IsActive(UserDelegationDto content)
    {
        return GetStatus(content) == "Active";
    }

    protected virtual async Task DelegatedImpersonate(UserDelegationDto context)
    {
        if (IsActive(context))
        {
            await JsRuntime.InvokeVoidAsync("eval", "document.getElementById('UserDelegationId').value = '" + context.Id + "'");
            await JsRuntime.InvokeVoidAsync("eval", "document.getElementById('DelegatedImpersonationForm').submit()");
        }
    }

    protected virtual async Task CloseModalAsync()
    {
        await _modal.HideAsync();
    }

    protected virtual async Task CloseDelegateNewUserModal()
    {
        await _delegateNewUserModal.HideAsync();
    }

    protected virtual async Task CloseConfirmationModal()
    {
        await _deleteConfirmationModal.HideAsync();
    }
}