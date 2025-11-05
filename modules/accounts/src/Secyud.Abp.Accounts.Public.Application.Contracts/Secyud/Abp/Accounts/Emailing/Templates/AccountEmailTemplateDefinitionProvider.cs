using Secyud.Abp.Account.Localization;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace Secyud.Abp.Accounts.Emailing.Templates;

public class AccountEmailTemplateDefinitionProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.Add(
            new TemplateDefinition(
                AccountEmailTemplates.PasswordResetLink,
                displayName: LocalizableString.Create<AbpAccountsResource>($"TextTemplate:{AccountEmailTemplates.PasswordResetLink}"),
                layout: StandardEmailTemplates.Layout,
                localizationResource: typeof(AbpAccountsResource)
            ).WithVirtualFilePath("/Secyud/Abp/Accounts/Emailing/Templates/PasswordResetLink.tpl", true)
        );

        context.Add(
            new TemplateDefinition(
                AccountEmailTemplates.EmailConfirmationLink,
                displayName: LocalizableString.Create<AbpAccountsResource>($"TextTemplate:{AccountEmailTemplates.EmailConfirmationLink}"),
                layout: StandardEmailTemplates.Layout,
                localizationResource: typeof(AbpAccountsResource)
            ).WithVirtualFilePath("/Secyud/Abp/Accounts/Emailing/Templates/EmailConfirmationLink.tpl", true)
        );

        context.Add(
            new TemplateDefinition(
                AccountEmailTemplates.EmailSecurityCode,
                displayName: LocalizableString.Create<AbpAccountsResource>($"TextTemplate:{AccountEmailTemplates.EmailSecurityCode}"),
                layout: StandardEmailTemplates.Layout,
                localizationResource: typeof(AbpAccountsResource)
            ).WithVirtualFilePath("/Secyud/Abp/Accounts/Emailing/Templates/EmailSecurityCode.tpl", true)
        );
    }
}
