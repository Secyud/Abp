using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Secyud.Abp.AspNetCore.Styles;

namespace Secyud.Abp.AspNetCore.Components;

public partial class MainLayout
{
    [Inject]
    private IOptions<SecitsThemeOptions> SectsThemeOptions { get; set; } = null!;
}