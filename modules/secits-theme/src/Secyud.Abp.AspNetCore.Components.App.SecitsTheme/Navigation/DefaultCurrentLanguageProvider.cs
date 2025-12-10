using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Secyud.Abp.AspNetCore.Navigation;

public class DefaultCurrentLanguageProvider : ICurrentLanguageProvider, ISingletonDependency
{
    protected LanguageInfo? CurrentCultureName { get; set; } 

    public Task<LanguageInfo?> GetCurrentLanguageAsync()
    {
        return Task.FromResult(CurrentCultureName);
    }

    public Task SetCurrentLanguageAsync(LanguageInfo? languageInfo)
    {
        CurrentCultureName = languageInfo;
        return Task.CompletedTask;
    }
}