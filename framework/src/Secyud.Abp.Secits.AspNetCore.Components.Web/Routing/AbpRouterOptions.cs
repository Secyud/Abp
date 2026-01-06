using System.Reflection;

namespace Secyud.Abp.Secits.AspNetCore.Components.Routing;

public class AbpRouterOptions
{
    public Assembly AppAssembly { get; set; } = null!;

    public RouterAssemblyList AdditionalAssemblies { get; } = [];
}
