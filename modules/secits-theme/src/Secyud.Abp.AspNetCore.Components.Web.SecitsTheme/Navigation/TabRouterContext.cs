using Microsoft.AspNetCore.Components;

namespace Secyud.Abp.AspNetCore.Navigation;

public class TabRouterContext(RenderFragment body)
{
    public RenderFragment Body { get; set; } = body;
}