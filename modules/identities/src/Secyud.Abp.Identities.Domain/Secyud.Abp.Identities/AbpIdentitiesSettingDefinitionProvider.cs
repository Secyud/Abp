using Secyud.Abp.Identities.Features;
using Secyud.Abp.Identities.Localization;
using Secyud.Abp.Identities.Settings;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Secyud.Abp.Identities;

public class AbpIdentitiesSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
           new SettingDefinition(IdentitiesSettingNames.TwoFactor.Behaviour,
                nameof(IdentitiesTwoFactorBehaviour.Optional),
                L("DisplayName:Abp.Identity.TwoFactorBehaviour"),
                L("Description:Abp.Identity.TwoFactorBehaviour"),
                isVisibleToClients: true),

            new SettingDefinition(IdentitiesSettingNames.TwoFactor.UsersCanChange,
                true.ToString(),
                L("DisplayName:Abp.Identity.UsersCanChange"),
                L("Description:Abp.Identity.UsersCanChange"),
                isVisibleToClients: true),

           new SettingDefinition(IdentitiesSettingNames.EnableLdapLogin, "false",
               L("DisplayName:Abp.Identity.EnableLdapLogin"),
               L("Description:Abp.Identity.EnableLdapLogin"),
               isVisibleToClients: true),

           new SettingDefinition(IdentitiesSettingNames.EnableOAuthLogin, "false",
                L("DisplayName:Abp.Identity.EnableOAuthLogin"),
                L("Description:Abp.Identity.EnableOAuthLogin"),
                isVisibleToClients: true),

           new SettingDefinition(IdentitiesSettingNames.OAuthLogin.Authority,
               null,
               L("DisplayName:Abp.Identity.Authority"),
               L("Description:Abp.Identity.Authority"),
               true),

           new SettingDefinition(IdentitiesSettingNames.OAuthLogin.ClientId,
               null,
               L("DisplayName:Abp.Identity.ClientId"),
               L("Description:Abp.Identity.ClientId"),
               true),

           new SettingDefinition(IdentitiesSettingNames.OAuthLogin.ClientSecret,
               null,
               L("DisplayName:Abp.Identity.ClientSecret"),
               L("Description:Abp.Identity.ClientSecret"),
               true),

           new SettingDefinition(IdentitiesSettingNames.OAuthLogin.Scope,
               null,
               L("DisplayName:Abp.Identity.Scope"),
               L("Description:Abp.Identity.Scope"),
               true),

           new SettingDefinition(IdentitiesSettingNames.OAuthLogin.RequireHttpsMetadata,
               "false",
               L("DisplayName:Abp.Identity.RequireHttpsMetadata"),
               L("Description:Abp.Identity.RequireHttpsMetadata"),
               true),

           new SettingDefinition(IdentitiesSettingNames.OAuthLogin.ValidateEndpoints,
               "false",
               L("DisplayName:Abp.Identity.ValidateEndpoints"),
               L("Description:Abp.Identity.ValidateEndpoints"),
               true),

           new SettingDefinition(IdentitiesSettingNames.OAuthLogin.ValidateIssuerName,
               "false",
               L("DisplayName:Abp.Identity.ValidateIssuerName"),
               L("Description:Abp.Identity.ValidateIssuerName"),
               true),

           new SettingDefinition(IdentitiesSettingNames.Session.PreventConcurrentLogin,
               nameof(IdentitiesPreventConcurrentLoginBehaviour.Disabled),
               L("DisplayName:Abp.Identity.PreventConcurrentLogin"),
               L("Description:Abp.Identity.PreventConcurrentLogin"),
               true)
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpIdentitiesResource>(name);
    }
}
