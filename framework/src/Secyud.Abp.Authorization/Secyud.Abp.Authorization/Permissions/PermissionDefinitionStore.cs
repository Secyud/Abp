using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionDefinitionStore : IPermissionDefinitionStore, ISingletonDependency
{
    protected IDictionary<string, PermissionGroupDefinition> PermissionGroupDefinitions => _lazyPermissionGroupDefinitions.Value;
    private readonly Lazy<Dictionary<string, PermissionGroupDefinition>> _lazyPermissionGroupDefinitions;

    protected IDictionary<string, PermissionDefinition> PermissionDefinitions => _lazyPermissionDefinitions.Value;
    private readonly Lazy<Dictionary<string, PermissionDefinition>> _lazyPermissionDefinitions;

    protected AbpPermissionOptions Options { get; }

    private readonly IServiceProvider _serviceProvider;

    public PermissionDefinitionStore(
        IServiceProvider serviceProvider,
        IOptions<AbpPermissionOptions> options)
    {
        _serviceProvider = serviceProvider;
        Options = options.Value;

        _lazyPermissionDefinitions = new Lazy<Dictionary<string, PermissionDefinition>>(
            CreatePermissionDefinitions,
            isThreadSafe: true
        );

        _lazyPermissionGroupDefinitions = new Lazy<Dictionary<string, PermissionGroupDefinition>>(
            CreatePermissionGroupDefinitions,
            isThreadSafe: true
        );
    }
    
    protected virtual Dictionary<string, PermissionDefinition> CreatePermissionDefinitions()
    {
        var permissions = new Dictionary<string, PermissionDefinition>();

        foreach (var groupDefinition in PermissionGroupDefinitions.Values)
        {
            foreach (var permission in groupDefinition.Permissions)
            {
                AddPermissionToDictionaryRecursively(permissions, permission);
            }
        }

        return permissions;
    }

    protected virtual void AddPermissionToDictionaryRecursively(
        Dictionary<string, PermissionDefinition> permissions,
        PermissionDefinition permission)
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

    protected virtual Dictionary<string, PermissionGroupDefinition> CreatePermissionGroupDefinitions()
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

    public virtual Task<PermissionDefinition?> GetOrNullAsync(string name)
    {
        return Task.FromResult(PermissionDefinitions.GetOrDefault(name));
    }
    
    public virtual Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync()
    {
        return Task.FromResult<IReadOnlyList<PermissionDefinition>>(
            PermissionDefinitions.Values.ToImmutableList()
        );
    }

    public virtual Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync()
    {
        return Task.FromResult<IReadOnlyList<PermissionGroupDefinition>>(
            PermissionGroupDefinitions.Values.ToImmutableList()
        );
    }
}