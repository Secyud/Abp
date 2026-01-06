using System.Text.Json.Nodes;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace Secyud.Abp.Authorization.SimpleStateChecking;

public class PermissionsSimpleStateCheckerSerializerContributor :
    ISimpleStateCheckerSerializerContributor,
    ISingletonDependency
{
    public const string CheckerShortName = "P";

    public string? SerializeToJson<TState>(ISimpleStateChecker<TState> checker)
        where TState : IHasSimpleStateCheckers<TState>
    {
        if (checker is not RequirePermissionsSimpleStateChecker<TState> permissionsSimpleStateChecker)
        {
            return null;
        }

        var jsonObject = new JsonObject
        {
            ["T"] = CheckerShortName,
            ["A"] = permissionsSimpleStateChecker.RequiresAll
        };

        var nameArray = new JsonArray();
        foreach (var permissionName in permissionsSimpleStateChecker.PermissionNames)
        {
            nameArray.Add(permissionName);
        }

        jsonObject["N"] = nameArray;
        return jsonObject.ToJsonString();
    }

    public ISimpleStateChecker<TState>? Deserialize<TState>(
        JsonObject jsonObject,
        TState state)
        where TState : IHasSimpleStateCheckers<TState>
    {
        if (jsonObject["T"]?.ToString() != CheckerShortName)
        {
            return null;
        }

        if (jsonObject["N"] is not JsonArray nameArray)
        {
            throw new AbpException("'N' is not an array in the serialized state checker! JsonObject: " +
                                   jsonObject.ToJsonString());
        }

        return new RequirePermissionsSimpleStateChecker<TState>(
            new RequirePermissionsSimpleBatchStateCheckerModel<TState>(
                state,
                nameArray.Select(x => x!.ToString()).ToArray(),
                (bool?)jsonObject["A"] ?? false
            )
        );
    }
}