using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Secyud.Abp.Account.Localization;
using Secyud.Abp.Identities;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace Secyud.Abp.Accounts;

public class ChangePasswordInput : IValidatableObject
{
    [DisableAuditing]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    public string CurrentPassword { get; set; }= "";

    [Required]
    [DisableAuditing]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    public string NewPassword { get; set; }= "";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CurrentPassword == NewPassword) 
        {
            var localizer = validationContext.GetRequiredService<IStringLocalizer<AbpAccountsResource>>();

            yield return new ValidationResult(
                localizer["NewPasswordSameAsOld"],
                [nameof(CurrentPassword), nameof(NewPassword)]
            );
        }
    }
}
