using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Secyud.Abp.Identities.Components;
using Secyud.Abp.Identities.Localization;
using Secyud.Abp.Identities.Navigation;
using Secyud.Abp.Identities.Settings;
using Secyud.Abp.Permissions.Components;
using Secyud.Secits.Blazor;
using Secyud.Secits.Blazor.Element;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Settings;

namespace Secyud.Abp.Identities.Users;

[Route(IdentitiesMenus.UsersUrl)]
[Authorize(IdentityPermissions.Users.DefaultName)]
public partial class Index
{
    [Inject]
    protected ISettingProvider SettingProvider { get; set; } = null!;

    [Inject]
    protected IPermissionChecker PermissionChecker { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected IIdentitySessionAppService IdentitySessionAppService { get; set; } = null!;

    [Inject]
    protected IJSRuntime JsRuntime { get; set; } = null!;

    [Inject]
    protected IOptions<AbpIdentitiesBlazorOptions> Options { get; set; } = null!;

    [Inject]
    protected IIdentityRoleAppService RoleAppService { get; set; } = null!;

    [Inject]
    protected UserManagementState UserManagementState { get; set; } = null!;

    protected const string PermissionProviderName = "U";

    protected PermissionsGrantModal PermissionsGrantModal = null!;

    protected IReadOnlyList<IdentityRoleDto> Roles = [];

    protected AssignedRoleViewModel[] NewUserRoles = [];

    protected AssignedRoleViewModel[] EditUserRoles = [];


    protected string ManagePermissionsPolicyName;

    protected bool HasImpersonationPermission;

    protected string ImpersonationPolicyName;

    protected SModal LockModal { get; set; } = null!;
    protected UserLockViewModel LockingUser { get; set; } = new();

    protected SModal ChangePasswordModal { get; set; } = null!;

    protected bool RandomPasswordGenerated { get; set; }

    protected ChangeUserPasswordViewModel ChangePasswordModel { get; set; } = null!;

    protected SModal TwoFactorModal { get; set; } = null!;

    protected SModal SessionsModal { get; set; } = null!;

    protected SModal SessionDetailModal { get; set; } = null!;

    protected IdentitySessionDto SessionDetail = new();

    protected SModal ClaimsModal { get; set; } = null!;

    protected IdentityUserViewDetailsModal ViewDetailsModal { get; set; } = null!;

    protected TwoFactorViewModel TwoFactorModel { get; set; } = new();

    protected ClaimsViewModel ClaimsModel { get; set; } = new();
    protected SGrid<IdentitySessionDto> SessionsGrid { get; set; } = null!;

    protected string? SelectedClaimValueText { get; set; }

    protected int SelectedClaimValueNumeric { get; set; }

    protected DateTime SelectedClaimValueDate { get; set; }

    protected bool SelectedClaimValueBool { get; set; }

    [Required]
    protected ClaimTypeDto? SelectedClaim { get; set; }

    protected bool HasManagePermissionsPermission { get; set; }

    protected bool RequireConfirmedEmail;

    protected bool ShowAdvancedFilters { get; set; }

    protected string ViewDetailsPolicyName;

    protected bool HasViewDetailsPermission { get; set; }

    protected AdvancedFilterInput AdvancedFilterInput { get; set; } = new();

    public bool IsEditCurrentUser { get; set; }

    public Index()
    {
        ObjectMapperContext = typeof(AbpIdentitiesBlazorModule);
        LocalizationResource = typeof(AbpIdentitiesResource);

        CreatePolicyName = IdentityPermissions.Users.Create.Name;
        UpdatePolicyName = IdentityPermissions.Users.Update.Name;
        DeletePolicyName = IdentityPermissions.Users.Delete.Name;
        ManagePermissionsPolicyName = IdentityPermissions.Users.ManagePermissions.Name;
        ImpersonationPolicyName = IdentityPermissions.Users.Impersonation.Name;
        ViewDetailsPolicyName = IdentityPermissions.Users.ViewDetails.Name;
    }

    protected override async Task UpdateGetListInputAsync(DataRequest request)
    {
        await base.UpdateGetListInputAsync(request);

        if (ShowAdvancedFilters)
        {
            GetListInput.OrganizationUnitId = AdvancedFilterInput.OrganizationUnitId;
            GetListInput.RoleId = AdvancedFilterInput.RoleId;
            GetListInput.NotActive = AdvancedFilterInput.NotActive;
            GetListInput.EmailConfirmed = AdvancedFilterInput.EmailConfirmed;
            GetListInput.IsLockedOut = AdvancedFilterInput.IsLockedOut;
            GetListInput.IsExternal = AdvancedFilterInput.IsExternal;
        }
        else
        {
            GetListInput.OrganizationUnitId = null;
            GetListInput.RoleId = null;
            GetListInput.NotActive = null;
            GetListInput.EmailConfirmed = null;
            GetListInput.IsLockedOut = null;
            GetListInput.IsExternal = null;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        UserManagementState.OnDataGridChanged += OnDataGridChangedAsync;
        UserManagementState.OnGetFilter += GetFilter;

        Roles = (await AppService.GetAssignableRolesAsync()).Items;
        RequireConfirmedEmail = await SettingProvider.IsTrueAsync(IdentitiesSettingNames.SignIn.RequireConfirmedEmail);
    }

    protected virtual async Task OnDataGridChangedAsync()
    {
        if (View is not null)
        {
            await View.RefreshAsync(true);
        }
    }

    protected virtual GetIdentityUsersInput GetFilter()
    {
        return GetListInput;
    }

    protected virtual void OnAdvancedFilterSectionClick()
    {
        ShowAdvancedFilters = !ShowAdvancedFilters;
    }

    protected Guid CurrentSessionUserId { get; set; }

    protected string? CurrentSessionUserName { get; set; }
    protected virtual int SessionPageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    protected virtual async Task<DataResult<IdentitySessionDto>> OnSessionsDataGridReadAsync(DataRequest e)
    {
        var sessions =
            await IdentitySessionAppService.GetListAsync(new GetIdentitySessionListInput
            {
                UserId = CurrentSessionUserId,
                SkipCount = e.SkipCount,
                MaxResultCount = e.PageSize
            });

        return new DataResult<IdentitySessionDto>
        {
            Items = sessions.Items,
            TotalCount = Convert.ToInt32(sessions.TotalCount)
        };
    }


    protected override async Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();

        HasManagePermissionsPermission = await AuthorizationService.IsGrantedAsync(ManagePermissionsPolicyName);
        HasImpersonationPermission = await AuthorizationService.IsGrantedAsync(ImpersonationPolicyName);
        HasViewDetailsPermission = await AuthorizationService.IsGrantedAsync(ViewDetailsPolicyName);
    }

    protected override async Task OpenCreateModalAsync()
    {
        NewUserRoles = Roles.Select(x => new AssignedRoleViewModel { Name = x.Name, IsAssigned = x.IsDefault }).ToArray();

        await base.OpenCreateModalAsync();

        CreateEntity.LockoutEnabled = true;
        CreateEntity.IsActive = true;
    }

    protected override Task CreateEntityAsync()
    {
        CreateEntity.RoleNames = NewUserRoles.Where(x => x.IsAssigned).Select(x => x.Name).ToArray();

        return base.CreateEntityAsync();
    }

    protected override async Task OpenUpdateModalAsync(IdentityUserDto entity)
    {
        IsEditCurrentUser = entity.Id == CurrentUser.Id;

        if (await PermissionChecker.IsGrantedAsync(IdentityPermissions.Users.ManageRoles.Name))
        {
            var userRoleNames = (await AppService.GetRolesAsync(entity.Id)).Items.Select(r => r.Name).ToList();

            EditUserRoles = Roles
                .Select(x => new AssignedRoleViewModel
                {
                    Name = x.Name,
                    IsAssigned = userRoleNames.Contains(x.Name),
                })
                .ToArray();
        }


        await base.OpenUpdateModalAsync(entity);
    }

    protected async Task CloseLockModalAsync()
    {
        LockingUser = new UserLockViewModel();
        await LockModal.HideAsync();
    }

    protected async Task CloseChangePasswordModalAsync()
    {
        ChangePasswordModel = new ChangeUserPasswordViewModel();
        await ChangePasswordModal.HideAsync();
    }

    protected async Task CloseTwoFactorModalAsync()
    {
        TwoFactorModel = new TwoFactorViewModel();
        await TwoFactorModal.HideAsync();
    }

    protected async Task CloseSessionsModalAsync()
    {
        await SessionsModal.HideAsync();
    }

    protected async Task CloseSessionDetailModalAsync()
    {
        await SessionDetailModal.HideAsync();
    }

    protected async Task CloseClaimsModalAsync()
    {
        ClaimsModel = new ClaimsViewModel();
        SelectedClaimValueText = null;
        await ClaimsModal.HideAsync();
    }

    protected async Task LockUserAsync()
    {
        try
        {
            await AppService.LockAsync(LockingUser.Id, LockingUser.LockoutEnd);

            await CloseLockModalAsync();
            await RefreshEntitiesAsync(false);
            await InvokeAsync(StateHasChanged);
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception e)
        {
            await Message.Error(e.Message);
        }
    }

    protected async Task ChangeTwoFactorAsync()
    {
        try
        {
            await AppService.SetTwoFactorEnabledAsync(TwoFactorModel.Id, TwoFactorModel.TwoFactorEnabled);

            await CloseTwoFactorModalAsync();
            await RefreshEntitiesAsync(false);
            await InvokeAsync(StateHasChanged);
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception e)
        {
            await Message.Error(e.Message);
        }
    }

    protected async Task ChangePasswordAsync()
    {
        try
        {
            await AppService.UpdatePasswordAsync(ChangePasswordModel.Id,
                new IdentityUserUpdatePasswordInput { NewPassword = ChangePasswordModel.NewPassword });
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception e)
        {
            await Message.Error(e.Message);
            return;
        }

        await CloseChangePasswordModalAsync();
    }

    protected async Task GenerateRandomPassword()
    {
        var rnd = new Random();
        var specials = "!*_#/+-.";
        var lowercase = "abcdefghjkmnpqrstuvwxyz";
        var uppercase = lowercase.ToUpper();
        var numbers = "23456789";

        var all = specials + lowercase + uppercase + numbers;

        var password =
            lowercase.ToArray().OrderBy(x => rnd.Next()).Take(rnd.Next(4, 6)).JoinAsString("") +
            specials.ToArray().OrderBy(x => rnd.Next()).Take(rnd.Next(1, 2)).JoinAsString("") +
            uppercase.ToArray().OrderBy(x => rnd.Next()).Take(rnd.Next(2, 3)).JoinAsString("") +
            numbers.ToArray().OrderBy(x => rnd.Next()).Take(rnd.Next(1, 2)).JoinAsString("");

        var requiredLength = 8;
        var requiredLengthSetting = await SettingProvider.GetOrNullAsync(IdentitiesSettingNames.Password.RequiredLength);
        if (requiredLengthSetting != null && int.TryParse(requiredLengthSetting, out var requiredLengthParsed))
        {
            requiredLength = requiredLengthParsed;
        }

        if (password.Length < requiredLength)
        {
            password += all.ToArray().OrderBy(x => rnd.Next()).Take(requiredLength - password.Length).JoinAsString("");
        }

        var requiredUniqueChars = 1;
        var requiredUniqueCharsSetting = await SettingProvider.GetOrNullAsync(IdentitiesSettingNames.Password.RequiredUniqueChars);
        if (requiredUniqueCharsSetting != null && int.TryParse(requiredUniqueCharsSetting, out var requiredUniqueCharsParsed))
        {
            requiredUniqueChars = requiredUniqueCharsParsed;
        }

        if (password.Distinct().Count() < requiredUniqueChars)
        {
            var uniqueChars = all.ToArray().OrderBy(x => rnd.Next()).Take(requiredUniqueChars - password.Distinct().Count()).JoinAsString("");
            password += uniqueChars;
        }

        ChangePasswordModel.NewPassword = password.ToArray().OrderBy(x => rnd.Next()).JoinAsString("");
        RandomPasswordGenerated = true;
    }

    protected virtual async Task AddClaimAsync()
    {
        var claim = SelectedClaim;
        if (claim is null)
        {
            return;
        }

        if (claim is { Required: true, ValueType: IdentityClaimValueType.String } && string.IsNullOrWhiteSpace(SelectedClaimValueText))
        {
            await Message.Info(L["ClaimValueCanNotBeBlank"]);
            return;
        }

        if (!SelectedClaimValueText.IsNullOrWhiteSpace() && !claim.Regex.IsNullOrWhiteSpace() && !Regex.IsMatch(SelectedClaimValueText, claim.Regex))
        {
            await Message.Info(L["ClaimValueIsInvalid", claim.Name]);
            return;
        }

        ClaimsModel.OwnedClaims.Add(new IdentityUserClaimViewModel
        {
            ClaimType = claim.Name,
            ClaimValueText = SelectedClaimValueText,
            ClaimValueNumeric = SelectedClaimValueNumeric,
            ClaimValueDate = SelectedClaimValueDate,
            ClaimValueBool = SelectedClaimValueBool
        });
    }

    protected IdentityClaimValueType GetClaimValueType(string claimType)
    {
        return ClaimsModel.AllClaims.FirstOrDefault(c => c.Name == claimType)?.ValueType ?? IdentityClaimValueType.String;
    }

    protected string GetClaimRegex(string claimType)
    {
        return ClaimsModel.AllClaims.FirstOrDefault(c => c.Name == claimType)?.Regex ?? string.Empty;
    }

    protected void RemoveClaim(IdentityUserClaimViewModel claim)
    {
        ClaimsModel.OwnedClaims.Remove(claim);
    }

    protected async Task SaveClaimsAsync()
    {
        var ownedClaimsToPost = new List<IdentityUserClaimDto>();

        foreach (var ownedClaim in ClaimsModel.OwnedClaims)
        {
            string? value;
            var claim = ClaimsModel.AllClaims.FirstOrDefault(c => c.Name == ownedClaim.ClaimType);

            if (claim is null)
            {
                continue;
            }

            switch (claim.ValueType)
            {
                case IdentityClaimValueType.String:
                    value = ownedClaim.ClaimValueText;
                    break;
                case IdentityClaimValueType.Int:
                    value = ownedClaim.ClaimValueNumeric.ToString();
                    break;
                case IdentityClaimValueType.Boolean:
                    value = ownedClaim.ClaimValueBool.ToString();
                    break;
                case IdentityClaimValueType.DateTime:
                    value = ownedClaim.ClaimValueDate.ToString(CultureInfo.InvariantCulture);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (value.IsNullOrWhiteSpace())
            {
                continue;
            }

            if (!claim.Regex.IsNullOrWhiteSpace() && !Regex.IsMatch(value, claim.Regex))
            {
                await Message.Info(L["ClaimValueIsInvalid", claim.Name]);
                return;
            }

            ownedClaimsToPost.Add(
                new IdentityUserClaimDto
                {
                    UserId = ClaimsModel.Id,
                    ClaimType = ownedClaim.ClaimType,
                    ClaimValue = value
                }
            );
        }

        await AppService.UpdateClaimsAsync(ClaimsModel.Id, ownedClaimsToPost);

        await Notify.Success(L["SavedSuccessfully"]);
        await CloseClaimsModalAsync();
    }

    protected override Task UpdateEntityAsync()
    {
        UpdateEntity.RoleNames = EditUserRoles.Where(x => x.IsAssigned).Select(x => x.Name).ToArray();
        return base.UpdateEntityAsync();
    }

    protected override string GetDeleteConfirmationMessage(IdentityUserDto entity)
    {
        return string.Format(L["UserDeletionConfirmationMessage"], entity.UserName);
    }


    protected async Task OpenClaimsModalAsync(IdentityUserDto dto)
    {
        var allClaims = await AppService.GetAllClaimTypesAsync();
        ClaimsModel = new ClaimsViewModel
        {
            Id = dto.Id,
            UserName = dto.UserName,
            AllClaims = allClaims,
            OwnedClaims = (await AppService.GetClaimsAsync(dto.Id))
                .Select(c =>
                    new IdentityUserClaimViewModel
                    {
                        ClaimType = c.ClaimType,
                        ClaimValueText = c.ClaimValue,
                        ClaimValueNumeric =
                            allClaims.FirstOrDefault(ac => ac.Name == c.ClaimType)?.ValueType ==
                            IdentityClaimValueType.Int
                                ? Convert.ToInt32(c.ClaimValue)
                                : 0,
                        ClaimValueDate =
                            allClaims.FirstOrDefault(ac => ac.Name == c.ClaimType)?.ValueType ==
                            IdentityClaimValueType.DateTime
                                ? DateTime.Parse(c.ClaimValue, CultureInfo.InvariantCulture)
                                : DateTime.Now,
                        ClaimValueBool = allClaims.FirstOrDefault(ac => ac.Name == c.ClaimType)?.ValueType ==
                            IdentityClaimValueType.Boolean && bool.Parse(c.ClaimValue)
                    }
                ).ToList()
        };

        SelectedClaim = ClaimsModel.AllClaims.FirstOrDefault();
        await ClaimsModal.ShowAsync();
    }

    protected async Task OpenLockModalAsync(IdentityUserDto dto)
    {
        LockingUser = new UserLockViewModel
        {
            Id = dto.Id,
            LockoutEnd = dto.LockoutEnd?.LocalDateTime ?? DateTime.Now.AddDays(7).Date
        };

        await LockModal.ShowAsync();
    }

    protected async Task UnlockUserAsync(IdentityUserDto dto)
    {
        await AppService.UnlockAsync(dto.Id);
        await Notify.Success(L["UserUnlocked"]);
        await RefreshEntitiesAsync(false);
        await InvokeAsync(StateHasChanged);
    }

    protected async Task OpenSetPasswordModalAsync(IdentityUserDto dto)
    {
        RandomPasswordGenerated = false;

        ChangePasswordModel = new ChangeUserPasswordViewModel
        {
            Id = dto.Id,
            UserName = dto.UserName
        };

        await ChangePasswordModal.ShowAsync();
    }

    protected async Task OpenTwoFactorModalAsync(IdentityUserDto dto)
    {
        TwoFactorModel = new TwoFactorViewModel
        {
            Id = dto.Id,
            UserName = dto.UserName,
            TwoFactorEnabled = dto.TwoFactorEnabled
        };

        await TwoFactorModal.ShowAsync();
    }

    protected async Task OpenSessionsModalAsync(IdentityUserDto dto)
    {
        CurrentSessionUserId = dto.Id;
        CurrentSessionUserName = dto.UserName;
        await SessionsModal.ShowAsync();
    }

    protected async Task LoginWithUserAsync(IdentityUserDto dto)
    {
        await JsRuntime.InvokeVoidAsync("eval",
            $"document.getElementById('ImpersonationUserId').value = '{dto.Id:D}'");
        await JsRuntime.InvokeVoidAsync("eval",
            "document.getElementById('ImpersonationForm').submit()");
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        UserManagementState.OnDataGridChanged -= OnDataGridChangedAsync;
        UserManagementState.OnGetFilter -= GetFilter;
    }


    protected static bool?[] BoolItems { get; } = [null, true, false];

    protected string BoolDisplayName(bool? b)
    {
        return b switch
        {
            null => "-", true => L["Yes"], false => L["No"]
        };
    }

    protected string BoolDisplayName(bool b)
    {
        return b switch
        {
            true => "True", false => "False"
        };
    }
}