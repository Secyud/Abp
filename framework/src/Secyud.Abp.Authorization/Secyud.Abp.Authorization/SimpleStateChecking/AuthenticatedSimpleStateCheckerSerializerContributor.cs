using System.Text.Json.Nodes;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace Secyud.Abp.Authorization.SimpleStateChecking;

public class AuthenticatedSimpleStateCheckerSerializerContributor :
    ISimpleStateCheckerSerializerContributor,
    ISingletonDependency
{
    public const string CheckerShortName = "A";

    public string? SerializeToJson<TState>(ISimpleStateChecker<TState> checker)
        where TState : IHasSimpleStateCheckers<TState>
    {
        if (checker is not RequireAuthenticatedSimpleStateChecker<TState>)
        {
            return null;
        }

        var jsonObject = new JsonObject
        {
            ["T"] = CheckerShortName
        };

        return jsonObject.ToJsonString();
    }

    public ISimpleStateChecker<TState>? Deserialize<TState>(JsonObject jsonObject, TState state)
        where TState : IHasSimpleStateCheckers<TState>
    {
        return jsonObject["T"]?.ToString() != CheckerShortName
            ? null
            : new RequireAuthenticatedSimpleStateChecker<TState>();
    }
}