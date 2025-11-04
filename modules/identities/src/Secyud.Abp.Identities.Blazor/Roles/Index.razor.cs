using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Secyud.Abp.Identities.Localization;
using Secyud.Abp.Identities.Navigation;
using Secyud.Abp.Permissions.Components;
using Secyud.Secits.Blazor;
using Secyud.Secits.Blazor.Element;

namespace Secyud.Abp.Identities.Roles;

[Authorize(IdentityPermissions.Roles.DefaultName)]
[Route(IdentitiesMenus.RolesUrl)]
public partial class Index
{
    public Index()
    {
        ObjectMapperContext = typeof(AbpIdentitiesBlazorModule);
        LocalizationResource = typeof(AbpIdentitiesResource);

        CreatePolicyName = IdentityPermissions.Roles.Create.Name;
        UpdatePolicyName = IdentityPermissions.Roles.Update.Name;
        DeletePolicyName = IdentityPermissions.Roles.Delete.Name;
        ManagePermissionsPolicyName = IdentityPermissions.Roles.ManagePermissions.Name;
    }

    protected SModal DeleteModal { get; set; } = null!;
    protected DeleteRoleViewModel DeletingRole { get; set; } = null!;
    protected SModal MoveAllUsersModal { get; set; } = null!;
    protected MoveAllUsersViewModel MoveAllUsersModel { get; set; } = null!;
    protected SGrid<IdentityRoleDto> Grid { get; set; } = null!;
    protected PermissionsGrantModal PermissionsGrantModal { get; set; } = null!;

    protected const string PermissionProviderName = "R";

    protected string ManagePermissionsPolicyName;
    protected bool HasManagePermissionsPermission { get; set; }

    protected override async Task OpenDeleteModalAsync(IdentityRoleDto data)
    {
        var role = await AppService.GetAsync(data.Id);
        var allRoles = await AppService.GetAllListAsync();
        DeletingRole = new DeleteRoleViewModel
        {
            Id = role.Id,
            Name = role.Name,
            UserCount = role.UserCount,
            OtherRoles = allRoles.Items.Where(x => x.Id != role.Id).ToList()
        };
        await DeleteModal.ShowAsync();
    }

    protected virtual Task CloseDeleteModalAsync()
    {
        return DeleteModal.HideAsync();
    }

    protected virtual async Task DeleteRoleAsync()
    {
        try
        {
            await CheckDeletePolicyAsync();
            await OnDeletingEntityAsync();
            if (DeletingRole.AssignToRoleId == Guid.Empty)
            {
                DeletingRole.AssignToRoleId = null;
            }

            await AppService.MoveAllUsersAsync(DeletingRole.Id, DeletingRole.AssignToRoleId);
            await AppService.DeleteAsync(DeletingRole.Id);
            await CloseDeleteModalAsync();
            await OnDeletedEntityAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OpenMoveAllUsersModalAsync(IdentityRoleDto role)
    {
        if (role.UserCount == 0)
        {
            await Message.Warn(L["ThereIsNoUsersCurrentlyInThisRole"]);
            return;
        }

        var allRoles = await AppService.GetAllListAsync();
        MoveAllUsersModel = new MoveAllUsersViewModel()
        {
            CurrentRoleId = role.Id,
            CurrentRoleName = role.Name,
            TargetRoles = allRoles.Items.Where(x => x.Id != role.Id).ToList()
        };

        await MoveAllUsersModal.ShowAsync();
    }

    protected virtual async Task CloseMoveAllUsersModalAsync()
    {
        await MoveAllUsersModal.HideAsync();
    }

    protected virtual async Task MoveAllUsersAsync()
    {
        try
        {
            await CheckUpdatePolicyAsync();
            if (MoveAllUsersModel.TargetRoleId == Guid.Empty)
            {
                MoveAllUsersModel.TargetRoleId = null;
            }

            await AppService.MoveAllUsersAsync(MoveAllUsersModel.CurrentRoleId, MoveAllUsersModel.TargetRoleId);
            await Grid.RefreshAsync(true);
            await CloseMoveAllUsersModalAsync();
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual void AssignCheckedChanged(bool value)
    {
        DeletingRole.AssignRole = value;
        if (DeletingRole.AssignRole)
        {
            DeletingRole.DisabledDeleteButton = true;
        }
        else
        {
            DeletingRole.AssignToRoleId = Guid.Empty;
            DeletingRole.DisabledDeleteButton = false;
        }
    }

    protected virtual void OnAssignToRoleSelectedValueChanged(Guid? id)
    {
        DeletingRole.AssignToRoleId = id;

        DeletingRole.DisabledDeleteButton = DeletingRole.AssignToRoleId == Guid.Empty;
    }

    protected override async Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();

        HasManagePermissionsPermission = await AuthorizationService.IsGrantedAsync(ManagePermissionsPolicyName);
    }

    protected override string GetDeleteConfirmationMessage(IdentityRoleDto entity)
    {
        return string.Format(L["RoleDeletionConfirmationMessage"], entity.Name);
    }


    private bool _updateRoleIsStatic;

    protected override async Task OpenUpdateModalAsync(IdentityRoleDto entity)
    {
        _updateRoleIsStatic = entity.IsStatic;
        await base.OpenUpdateModalAsync(entity);
    }


    public class DeleteRoleViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";

        public long UserCount { get; set; }

        public List<IdentityRoleDto> OtherRoles { get; set; } = [];

        public bool AssignRole { get; set; }

        public Guid? AssignToRoleId { get; set; } = Guid.Empty;

        public bool DisabledDeleteButton { get; set; }
    }


    public class MoveAllUsersViewModel
    {
        public Guid CurrentRoleId { get; set; }

        public string CurrentRoleName { get; set; } = "";

        public List<IdentityRoleDto> TargetRoles { get; set; } = [];

        public Guid? TargetRoleId { get; set; }
    }
}