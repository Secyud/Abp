using System.Reflection;

namespace Secyud.Abp.AspNetCore.Routing;

public class AbpRouterOptions
{
    public Assembly AppAssembly { get; set; } = null!;

    public RouterAssemblyList AdditionalAssemblies { get; } = [];
}
