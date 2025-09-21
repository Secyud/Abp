using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace Secyud.Abp.AspNetCore.Components;

public partial class MainLayout
{
    [Inject]
    private IOptions<SecitsThemeOptions> SectsThemeOptions { get; set; } = null!;
}