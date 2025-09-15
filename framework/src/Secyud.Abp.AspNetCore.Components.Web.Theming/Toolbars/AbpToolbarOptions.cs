using JetBrains.Annotations;

namespace Secyud.Abp.AspNetCore.Toolbars;

public class AbpToolbarOptions
{
    public List<IToolbarContributor> Contributors { get; } = new();
}
