using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Secyud.Abp.AspNetCore.Styles;
using Secyud.Secits.Blazor;
using Secyud.Secits.Blazor.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;
using Volo.Abp.Localization;

namespace Secyud.Abp.AspNetCore.TagHeapers;

[HtmlTargetElement("secits-theme-style", TagStructure = TagStructure.NormalOrSelfClosing)]
public class SecitsThemeStylesTagHelper : AbpTagHelper
{
    [HtmlAttributeNotBound] [ViewContext] public ViewContext ViewContext { get; set; } = null!;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;
        output.Content.Clear();

        var leptonXStyleProvider = ViewContext.HttpContext.RequestServices.GetRequiredService<SecitsStyleProvider>();
        var urlHelper = ViewContext.GetUrlHelper();
        var selectedStyle = await leptonXStyleProvider.GetCurrentStyleAsync();
        var options = ViewContext.HttpContext.RequestServices.GetRequiredService<IOptions<SecitsThemeOptions>>().Value;

        var option = options.GenerateThemeInput(selectedStyle);

        var styleOptions = ViewContext.HttpContext.RequestServices
            .GetRequiredService<IOptions<SecitsStylesOptions>>()
            .Value;

        var resources = styleOptions.Get(option);
        foreach (var resource in resources)
        {
            output.Content.AppendHtml(GetLinkHtml(urlHelper, resource));
        }
    }

    private static string GetLinkHtml(IUrlHelper urlHelper, SecitsStyleFile resource)
    {
        var url = urlHelper.Content($"~/{resource.Path}");
        return
            $"""<link href="{url}" type="text/css" rel="stylesheet" theme="secits" id="{resource.Id}"/>{Environment.NewLine}""";
    }
}