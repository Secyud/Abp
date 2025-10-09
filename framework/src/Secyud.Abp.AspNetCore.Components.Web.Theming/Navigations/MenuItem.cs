using Microsoft.Extensions.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.UI.Navigation;

namespace Secyud.Abp.AspNetCore.Components.Navigations;

public class MenuItem(string name, string? displayName = null, MenuItem? parent = null)
{
    public string Name { get; } = parent is null ? name : $"{parent.Name}.{name}";
    public string DisplayName { get; } = "Menu:" + (displayName ?? name);
    public string? Icon { get; init; }
    public string? Url { get; init; }
    public int Order { get; init; } = 1000;
    public string? Target { get; init; }
    public string? CssClass { get; init; }
    public string? ElementId { get; init; }
    public string? GroupName { get; init; }
    public bool RequireAuthentication { get; init; }
    public string[]? RequiredPermissions { get; init; }
    public string[]? RequiredFeatures { get; init; }
    public string[]? RequireGlobalFeatures { get; init; }

    public ApplicationMenuItem Create(IStringLocalizer l)
    {
        var res = new ApplicationMenuItem(Name, l[DisplayName], Url, Icon, Order, Target, CssClass, ElementId, GroupName);
        if (!RequiredPermissions.IsNullOrEmpty()) res = res.RequirePermissions(RequiredPermissions!);
        if (!RequiredFeatures.IsNullOrEmpty()) res = res.RequireFeatures(RequiredFeatures!);
        if (!RequireGlobalFeatures.IsNullOrEmpty()) res = res.RequireGlobalFeatures(RequireGlobalFeatures!);
        if (RequireAuthentication) res = res.RequireAuthenticated();

        return res;
    }
}