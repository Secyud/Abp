using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Secits.Blazor.Validations;
using Secyud.Abp.Secits.Blazor.Validations.Formatters;
using Secyud.Secits.Blazor;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Authorization;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Secits.Blazor;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpGlobalFeaturesModule),
    typeof(AbpFeaturesModule)
)]
public class AbpSecitsBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureSecitsBlazor(context);
    }

    private void ConfigureSecitsBlazor(ServiceConfigurationContext context)
    {
        context.Services.AddSecitsBlazor();

        context.Services.Configure<AbpSecitsValidationOptions>(options =>
        {
            options.AddFormatter<CompareAttribute>((a, t, n) => string.Format(t, n, a.OtherPropertyDisplayName ?? a.OtherProperty));
            options.AddFormatter<FileExtensionsAttribute>((a, t, n) => string.Format(t, n,
                a.Extensions.Replace(" ", string.Empty).Replace(".", string.Empty).ToLowerInvariant()
                    .Split(',').Select(e => "." + e).Aggregate((left, right) => left + ", " + right)));
            options.AddFormatter<LengthAttribute>((a, t, n) => string.Format(t, n, a.MinimumLength, a.MaximumLength));
            options.AddFormatter<MaxLengthAttribute>((a, t, n) => string.Format(t, n, a.Length));
            options.AddFormatter<MinLengthAttribute>((a, t, n) => string.Format(t, n, a.Length));
            options.AddFormatter<RangeAttribute>((a, t, n) => string.Format(t, n, a.Minimum, a.Maximum));
            options.AddFormatter<RegularExpressionAttribute>((a, t, n) => string.Format(t, n, a.Pattern));
            options.AddFormatter<StringLengthAttribute>((a, t, n) => string.Format(t, n, a.MinimumLength, a.MaximumLength));
        });
    }
}