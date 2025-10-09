using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Permissions;

public class PermissionFinder(IPermissionStore permissionStore) : IPermissionFinder, ITransientDependency
{
    protected IPermissionStore PermissionStore { get; } = permissionStore;

    public virtual async Task<List<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> requests)
    {
        var result = new List<IsGrantedResponse>();
        foreach (var item in requests)
        {
            result.Add(new IsGrantedResponse
            {
                UserId = item.UserId,
                Permissions = (await PermissionStore.IsGrantedAsync(item.PermissionNames, UserPermissionValueProvider.ProviderName, item.UserId.ToString()))
                    .Result.ToDictionary(x => x.Key, x => x.Value == PermissionGrantResult.Granted)
            });
        }

        return result;
    }
}