using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace Secyud.Abp.Permissions;

public class StaticPermissionSaver(
    IStaticPermissionDefinitionStore staticStore,
    IPermissionGroupDefinitionRecordRepository permissionGroupRepository,
    IPermissionDefinitionRecordRepository permissionRepository,
    IPermissionDefinitionSerializer permissionSerializer,
    IOptions<AbpDistributedCacheOptions> cacheOptions,
    IApplicationInfoAccessor applicationInfoAccessor,
    IAbpDistributedLock distributedLock,
    IOptions<AbpPermissionOptions> permissionOptions,
    IUnitOfWorkManager unitOfWorkManager,
    IDistributedEventBus distributedEventBus)
    : IStaticPermissionSaver, ITransientDependency
{
    protected IStaticPermissionDefinitionStore StaticStore { get; } = staticStore;
    protected IPermissionGroupDefinitionRecordRepository PermissionGroupRepository { get; } = permissionGroupRepository;
    protected IPermissionDefinitionRecordRepository PermissionRepository { get; } = permissionRepository;
    protected IPermissionDefinitionSerializer PermissionSerializer { get; } = permissionSerializer;
    protected IApplicationInfoAccessor ApplicationInfoAccessor { get; } = applicationInfoAccessor;
    protected IAbpDistributedLock DistributedLock { get; } = distributedLock;
    protected AbpPermissionOptions PermissionOptions { get; } = permissionOptions.Value;
    protected AbpDistributedCacheOptions CacheOptions { get; } = cacheOptions.Value;
    protected IUnitOfWorkManager UnitOfWorkManager { get; } = unitOfWorkManager;
    protected IDistributedEventBus DistributedEventBus { get; } = distributedEventBus;

    public async Task SaveAsync()
    {
        await using var applicationLockHandle = await DistributedLock.TryAcquireAsync(
            GetApplicationDistributedLockKey()
        );

        if (applicationLockHandle == null)
        {
            /* Another application instance is already doing it */
            return;
        }

        var (permissionGroupRecords, permissionRecords) = await PermissionSerializer.SerializeAsync(
            await StaticStore.GetGroupsAsync()
        );

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
            await UpdateChangedPermissionGroupsAsync(permissionGroupRecords);
            await UpdateChangedPermissionsAsync(permissionRecords);
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

    private async Task UpdateChangedPermissionGroupsAsync(IEnumerable<PermissionGroupDefinitionRecord> groups)
    {
        var createRecords = new HashSet<PermissionGroupDefinitionRecord>();
        var updateRecords = new HashSet<PermissionGroupDefinitionRecord>();


        var databaseGroupDict = (await PermissionGroupRepository.GetListAsync())
            .ToDictionary(group => group.Name);
        var deleteGroupKeys = PermissionOptions.DeletedPermissionGroups;

        foreach (var group in groups)
        {
            var groupInDatabase = databaseGroupDict.GetOrDefault(group.Name);
            if (groupInDatabase is null)
            {
                if (!deleteGroupKeys.Contains(group.Name))
                    createRecords.Add(group);
                continue;
            }

            if (group.HasSameData(groupInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            groupInDatabase.Patch(group);
            updateRecords.Add(groupInDatabase);
        }

        /* Deleted */
        var deleteRecords = PermissionOptions.DeletedPermissionGroups.Count != 0
            ? databaseGroupDict.Values.Where(x =>
                    PermissionOptions.DeletedPermissionGroups.Contains(x.Name))
                .ToList()
            : [];

        if (createRecords.Count != 0)
        {
            await PermissionGroupRepository.InsertManyAsync(createRecords);
        }

        if (updateRecords.Count != 0)
        {
            await PermissionGroupRepository.UpdateManyAsync(updateRecords);
        }

        if (deleteRecords.Count != 0)
        {
            await PermissionGroupRepository.DeleteManyAsync(deleteRecords);
        }
    }

    private async Task UpdateChangedPermissionsAsync(IEnumerable<PermissionDefinitionRecord> permissions)
    {
        var createRecords = new List<PermissionDefinitionRecord>();
        var updateRecords = new List<PermissionDefinitionRecord>();

        var permissionsInDatabase = (await PermissionRepository.GetListAsync())
            .ToDictionary(x => x.Name);

        foreach (var permission in permissions)
        {
            var permissionInDatabase = permissionsInDatabase.GetOrDefault(permission.Name);
            if (permissionInDatabase == null)
            {
                /* New permission */
                createRecords.Add(permission);
                continue;
            }

            if (permission.HasSameData(permissionInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            permissionInDatabase.Patch(permission);
            updateRecords.Add(permissionInDatabase);
        }

        /* Deleted */
        var deleteRecords = new List<PermissionDefinitionRecord>();
        var deleteGroupKeys = PermissionOptions.DeletedPermissionGroups;
        var deletePermissionKeys = PermissionOptions.DeletedPermissions;

        if (PermissionOptions.DeletedPermissions.Count != 0)
        {
            deleteRecords.AddRange(permissionsInDatabase.Values
                .Where(x => deletePermissionKeys.Contains(x.Name))
            );
        }

        if (PermissionOptions.DeletedPermissionGroups.Count != 0)
        {
            deleteRecords.AddIfNotContains(permissionsInDatabase.Values
                .Where(x => deleteGroupKeys.Contains(x.GroupName))
            );
        }

        if (createRecords.Count != 0)
        {
            await PermissionRepository.InsertManyAsync(createRecords);
        }

        if (updateRecords.Count != 0)
        {
            await PermissionRepository.UpdateManyAsync(updateRecords);
        }

        if (deleteRecords.Count != 0)
        {
            await PermissionRepository.DeleteManyAsync(deleteRecords);
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