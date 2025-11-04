using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Secyud.Abp.Identities.Localization;
using Volo.Abp;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Localization;

namespace Secyud.Abp.Identities;

public class AbpIdentityResultException(IdentityResult identityResult) : 
    BusinessException(
        code: $"Secyud.Abp.Identity:{identityResult.Errors.First().Code}",
        message: identityResult.Errors.Select(err => err.Description).JoinAsString(", ")), ILocalizeErrorMessage
{
    public IdentityResult IdentityResult { get; } = Check.NotNull(identityResult, nameof(identityResult));

    public virtual string LocalizeMessage(LocalizationContext context)
    {
        var localizer = context.LocalizerFactory.Create<AbpIdentitiesResource>();

        SetData(localizer);

        return IdentityResult.LocalizeErrors(localizer);
    }

    protected virtual void SetData(IStringLocalizer localizer)
    {
        var values = IdentityResult.GetValuesFromErrorMessage(localizer);

        for (var index = 0; index < values.Length; index++)
        {
            Data[index.ToString()] = values[index];
        }
    }
}
