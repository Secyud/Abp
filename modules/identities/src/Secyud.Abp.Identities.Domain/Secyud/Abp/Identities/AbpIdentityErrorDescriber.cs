using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Secyud.Abp.Identities.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Secyud.Abp.Identities;

[Dependency(ServiceLifetime.Scoped, ReplaceServices = true)]
[ExposeServices(typeof(IdentityErrorDescriber))]
public class AbpIdentityErrorDescriber(IStringLocalizer<AbpIdentitiesResource> localizer) : IdentityErrorDescriber
{
    protected IStringLocalizer<AbpIdentitiesResource> Localizer { get; } = localizer;

    public override IdentityError InvalidUserName(string? userName)
    {
        using (CultureHelper.Use("en"))
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = Localizer["Secyud.Abp.Identity:InvalidUserName", userName ?? ""]
            };
        }
    }
}