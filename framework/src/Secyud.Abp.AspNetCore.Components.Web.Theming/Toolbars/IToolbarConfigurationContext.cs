using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.AspNetCore.Toolbars;

public interface IToolbarConfigurationContext : IServiceProviderAccessor
{
    Toolbar Toolbar { get; }

    IAuthorizationService AuthorizationService { get; }

    IStringLocalizerFactory StringLocalizerFactory { get; }

    Task<bool> IsGrantedAsync(string policyName);

    IStringLocalizer? GetDefaultLocalizer();

    [NotNull]
    public IStringLocalizer GetLocalizer<T>();

    [NotNull]
    public IStringLocalizer GetLocalizer(Type resourceType);
}
