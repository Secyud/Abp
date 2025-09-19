using System.Reflection;

namespace Secyud.Abp.AspNetCore.Components.Routing;

public class AbpRouterOptions
{
    public Assembly AppAssembly { get; set; } = null!;

    public RouterAssemblyList AdditionalAssemblies { get; } = [];
}
