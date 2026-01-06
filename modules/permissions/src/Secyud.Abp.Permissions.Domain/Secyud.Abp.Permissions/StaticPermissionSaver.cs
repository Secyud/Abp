using Microsoft.Extensions.Options;
using Secyud.Abp.Authorization.Permissions;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace Secyud.Abp.Permissions;

public class StaticPermissionSaver(
    IPermissionDefinitionStore store,
    IPermissionDefinitionRecordRepository permissionDefinitionRecordRepository,
    IPermissionGrantRepository permissionGrantRepository,
    IPermissionDefinitionSerializer permissionSerializer,
    IOptions<AbpDistributedCacheOptions> cacheOptions,
    IApplicationInfoAccessor applicationInfoAccessor,
    IAbpDistributedLock distributedLock,
    IOptions<PermissionsOptions> permissionsOptions,
    IUnitOfWorkManager unitOfWorkManager,
    IDistributedEventBus distributedEventBus)
    : IStaticPermissionSaver, ITransientDependency
{
    protected IPermissionDefinitionStore Store { get; } = store;
    protected IPermissionDefinitionRecordRepository PermissionDefinitionRecordRepository { get; } = permissionDefinitionRecordRepository;
    protected IPermissionGrantRepository PermissionGrantRepository { get; } = permissionGrantRepository;
    protected IPermissionDefinitionSerializer PermissionSerializer { get; } = permissionSerializer;
    protected IApplicationInfoAccessor ApplicationInfoAccessor { get; } = applicationInfoAccessor;
    protected IAbpDistributedLock DistributedLock { get; } = distributedLock;
    protected PermissionsOptions PermissionsOptions { get; } = permissionsOptions.Value;
    protected AbpDistributedCacheOptions CacheOptions { get; } = cacheOptions.Value;
    protected IUnitOfWorkManager UnitOfWorkManager { get; } = unitOfWorkManager;
    protected IDistributedEventBus DistributedEventBus { get; } = distributedEventBus;

    public async Task SaveAsync()
    {
        await using var applicationLockHandle = await DistributedLock
            .TryAcquireAsync(GetApplicationDistributedLockKey());

        if (applicationLockHandle == null)
        {
            /* Another application instance is already doing it */
            return;
        }

        await using var commonLockHandle = await DistributedLock.TryAcquireAsync(
            GetCommonDistributedLockKey(),
            TimeSpan.FromMinutes(5));

        if (commonLockHandle == null)
        {
            /* It will re-try */
            throw new AbpException("Could not acquire distributed lock for saving static permissions!");
        }

        using var unitOfWork = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true);
        try
        {
            await UpdateChangedPermissionsAsync();
        }
        catch
        {
            try
            {
                await unitOfWork.RollbackAsync();
            }
            catch
            {
                /* ignored */
            }

            throw;
        }

        await unitOfWork.CompleteAsync();
    }


    private async Task UpdateChangedPermissionsAsync()
    {
        var permissions = await Store.GetPermissionsAsync();

        var createRecords = new List<PermissionDefinitionRecord>();
        var updateRecords = new List<PermissionDefinitionRecord>();
        var deleteRecords = new List<PermissionDefinitionRecord>();

        var permissionsInDatabase = new Dictionary<string, PermissionDefinitionRecord>();

        foreach (var permission in permissions.Where(u => u.ParentName is null))
        {
            var list = await PermissionDefinitionRecordRepository.GetListAsync(permission.Name);
            foreach (var record in list)
            {
                permissionsInDatabase[record.Name] = record;
            }
        }

        foreach (var permission in permissions)
        {
            var record = await PermissionSerializer.SerializeAsync(permission);
            var permissionInDatabase = permissionsInDatabase.GetOrDefault(record.Name);
            if (permissionInDatabase is null)
            {
                /* New permission */
                createRecords.Add(record);
                continue;
            }

            if (record.HasSameData(permissionInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            permissionInDatabase.Patch(record);
            updateRecords.Add(permissionInDatabase);
            permissionsInDatabase.Remove(record.Name);
        }


        if (PermissionsOptions.UpdatePermissionsPrefixes is { Count: > 0 } prefixes)
        {
            deleteRecords.AddRange(permissionsInDatabase.Values
                .Where(x => prefixes.Any(u => x.Name.StartsWith(u)))
            );
        }

        if (createRecords.Count != 0)
        {
            await PermissionDefinitionRecordRepository.InsertManyAsync(createRecords);
        }

        if (updateRecords.Count != 0)
        {
            await PermissionDefinitionRecordRepository.UpdateManyAsync(updateRecords);
        }

        if (deleteRecords.Count != 0)
        {
            await PermissionDefinitionRecordRepository.DeleteManyAsync(deleteRecords);
            var deletedPermissionNames = deleteRecords.Select(u => u.Name).ToList();
            await PermissionGrantRepository.DeleteDirectAsync(u => deletedPermissionNames.Contains(u.Name));
        }

        if (createRecords.Count != 0 || updateRecords.Count != 0 || deleteRecords.Count != 0)
        {
            await DistributedEventBus.PublishAsync(new DynamicPermissionDefinitionsChangedEto
            {
                CreatedPermissions = createRecords.Select(u => u.Name).ToList(),
                UpdatedPermissions = updateRecords.Select(u => u.Name).ToList(),
                DeletedPermissions = deleteRecords.Select(u => u.Name).ToList(),
            });
        }
    }

    private string GetApplicationDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpPermissionUpdateLock";
    }

    private string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpPermissionUpdateLock";
    }
}