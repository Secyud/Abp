using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionDefinitionStore : IPermissionDefinitionStore, ISingletonDependency
{
    protected IDictionary<string, IPermissionDefinition> PermissionGroupDefinitions => _lazyPermissionGroupDefinitions.Value;
    private readonly Lazy<Dictionary<string, IPermissionDefinition>> _lazyPermissionGroupDefinitions;

    protected IDictionary<string, IPermissionDefinition> PermissionDefinitions => _lazyPermissionDefinitions.Value;
    private readonly Lazy<Dictionary<string, IPermissionDefinition>> _lazyPermissionDefinitions;

    protected AbpPermissionOptions Options { get; }

    private readonly IServiceProvider _serviceProvider;

    public PermissionDefinitionStore(
        IServiceProvider serviceProvider,
        IOptions<AbpPermissionOptions> options)
    {
        _serviceProvider = serviceProvider;
        Options = options.Value;

        _lazyPermissionDefinitions = new Lazy<Dictionary<string, IPermissionDefinition>>(
            CreatePermissionDefinitions,
            isThreadSafe: true
        );

        _lazyPermissionGroupDefinitions = new Lazy<Dictionary<string, IPermissionDefinition>>(
            CreatePermissionGroupDefinitions,
            isThreadSafe: true
        );
    }
    
    protected virtual Dictionary<string, IPermissionDefinition> CreatePermissionDefinitions()
    {
        var permissions = new Dictionary<string, IPermissionDefinition>();

        foreach (var groupDefinition in PermissionGroupDefinitions.Values)
        {
            foreach (var permission in groupDefinition.Children)
            {
                AddPermissionToDictionaryRecursively(permissions, permission);
            }
        }

        return permissions;
    }

    protected virtual void AddPermissionToDictionaryRecursively(
        Dictionary<string, IPermissionDefinition> permissions,
        IPermissionDefinition permission)
    {
        if (!permissions.TryAdd(permission.Name, permission))
        {
            throw new AbpException("Duplicate permission name: " + permission.Name);
        }

        foreach (var child in permission.Children)
        {
            AddPermissionToDictionaryRecursively(permissions, child);
        }
    }

    protected virtual Dictionary<string, IPermissionDefinition> CreatePermissionGroupDefinitions()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = new PermissionDefinitionContext(scope.ServiceProvider);

        var providers = Options
            .DefinitionProviders
            .Select(p => (scope.ServiceProvider.GetRequiredService(p) as IPermissionDefinitionProvider)!)
            .ToList();

        foreach (var provider in providers)
        {
            provider.PreDefine(context);
        }

        foreach (var provider in providers)
        {
            provider.Define(context);
        }

        foreach (var provider in providers)
        {
            provider.PostDefine(context);
        }

        return context.Groups;
    }

    public virtual Task<IPermissionDefinition?> GetOrNullAsync(string name)
    {
        return Task.FromResult(PermissionDefinitions.GetOrDefault(name));
    }
    
    public virtual Task<IReadOnlyList<IPermissionDefinition>> GetPermissionsAsync()
    {
        return Task.FromResult<IReadOnlyList<IPermissionDefinition>>(
            PermissionDefinitions.Values.ToImmutableList()
        );
    }

    public virtual Task<IReadOnlyList<IPermissionDefinition>> GetGroupsAsync()
    {
        return Task.FromResult<IReadOnlyList<IPermissionDefinition>>(
            PermissionGroupDefinitions.Values.ToImmutableList()
        );
    }
}