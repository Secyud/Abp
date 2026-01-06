using Microsoft.AspNetCore.Components;
using Secyud.Abp.Permissions.Localization;
using Secyud.Secits.Blazor;
using Secyud.Secits.Blazor.Element;
using Secyud.Secits.Blazor.Icons;
using Volo.Abp.AspNetCore.Components.Web.Configuration;

namespace Secyud.Abp.Permissions.Components;

public partial class PermissionsGrantModal
{
    public PermissionsGrantModal()
    {
        LocalizationResource = typeof(AbpPermissionsResource);
    }

    [Inject] protected IIconProvider IconProvider { get; set; } = null!;

    [Inject] protected IPermissionAppService PermissionAppService { get; set; } = null!;

    [Inject]
    protected ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService
    {
        get;
        set;
    } = null!;


    private string? _crossIcon;

    protected SPopup? Modal { get; set; }

    protected List<PermissionGroupInfoModel>? Groups { get; set; }

    protected List<PermissionGroupInfoModel> ShowGroups { get; } = [];
    protected string? ProviderName { get; set; }
    protected string? ProviderKey { get; set; }
    protected string? PermissionGrantSearchText { get; set; }
    protected string? EntityDisplayName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _crossIcon = IconProvider.GetIcon(IconName.Cross);
        await Task.CompletedTask;
    }

    protected virtual void SetGroupTab(Tab<PermissionGroupInfoModel> tab)
    {
        tab.Click = () => CheckGroupPermissionsAsync(tab.Item);
    }

    protected virtual async Task CheckGroupPermissionsAsync(PermissionGroupInfoModel model)
    {
        if (model.Permissions is not null) return;

        try
        {
            var permissions = await PermissionAppService
                .GetListAsync(model.Name, ProviderName!, ProviderKey!);
            var permissionModels = new List<PermissionGrantInfoModel>();
            model.Permissions = ObjectMapper.Map(permissions, permissionModels);
            var dictionary = model.Permissions
                .ToDictionary(m => m.Name, m => m);

            foreach (var permissionModel in permissionModels)
            {
                permissionModel.Group = model;
                if (permissionModel.ParentName is { } parentName &&
                    dictionary.GetValueOrDefault(parentName) is { } parent)
                {
                    permissionModel.Parent = parent;
                    parent.Children ??= [];
                    parent.Children.Add(permissionModel);
                }
            }

            model.RootPermissions = model.Permissions
                .Where(u => u.Parent is null).ToList();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task CheckPermissionChildrenAsync(PermissionGrantInfoModel model)
    {
        model.Children ??= [];
        await Task.CompletedTask;
    }

    protected virtual async Task OnPermissionInfoModelCheckClickAsync(PermissionGrantInfoModel model)
    {
        if (model.IsChecked)
            UncheckPermissionGrantInfoModel(model);
        else
            CheckPermissionGrantInfoModel(model);

        await InvokeAsync(StateHasChanged);
    }

    protected virtual void CheckPermissionGrantInfoModel(PermissionGrantInfoModel? model)
    {
        while (model is not null)
        {
            model.IsChecked = true;
            model = model.Parent;
        }
    }

    protected virtual void UncheckPermissionGrantInfoModel(PermissionGrantInfoModel? model)
    {
        if (model is null) return;
        var queue = new Queue<PermissionGrantInfoModel>();
        queue.Enqueue(model);
        while (queue.Count > 0)
        {
            model = queue.Dequeue();
            model.IsChecked = false;
            if (model.Children is null) continue;
            foreach (var child in model.Children)
            {
                queue.Enqueue(child);
            }
        }
    }

    public virtual async Task OpenAsync(string providerName, string providerKey, string entityDisplayName)
    {
        try
        {
            ProviderName = providerName;
            ProviderKey = providerKey;
            EntityDisplayName = entityDisplayName;
            PermissionGrantSearchText = null;

            var groups = await PermissionAppService.GetGroupsAsync();
            Groups = ObjectMapper.Map(groups, Groups);
            ShowGroups.Clear();
            if (Groups is not null)
                ShowGroups.AddRange(Groups);

            await Modal!.ShowAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected Task CloseModalAsync()
    {
        return Modal?.HideAsync() ?? Task.CompletedTask;
    }

    protected virtual async Task SaveAsync()
    {
        try
        {
            var updateDto = new UpdatePermissionsDto();
            Queue<PermissionGrantInfoModel> queue = new();
            var hasPermission = false;
            foreach (var group in Groups!)
            {
                AddPermissions(group.Permissions);
            }

            while (queue.Count > 0)
            {
                var model = queue.Dequeue();
                AddPermissions(model.Children);
            }

            if (!hasPermission &&
                !await Message.Confirm(L["SaveWithoutAnyPermissionsWarningMessage"].Value))
            {
                return;
            }

            await PermissionAppService.UpdateAsync(ProviderName!, ProviderKey!, updateDto);
            await CurrentApplicationConfigurationCacheResetService.ResetAsync();
            await Modal!.HideAsync();
            await Notify.Success(L["SavedSuccessfully"]);

            void AddPermissions(List<PermissionGrantInfoModel>? permissions)
            {
                if (permissions is null || permissions.Count == 0) return;

                foreach (var permission in permissions)
                {
                    hasPermission = hasPermission && permission.IsChecked;
                    queue.Enqueue(permission);
                    if (permission.IsGranted != permission.IsChecked)
                        updateDto.Permissions.Add(new UpdatePermissionDto
                        {
                            Name = permission.Name,
                            IsGranted = permission.IsGranted
                        });
                }
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
}