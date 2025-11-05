using Secyud.Abp.Account.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;

namespace Secyud.Abp.Account.Feature;

public class AccountFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(AccountFeature.GroupName,
            L("Feature:AccountGroup"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpAccountsResource>(name);
    }
}
