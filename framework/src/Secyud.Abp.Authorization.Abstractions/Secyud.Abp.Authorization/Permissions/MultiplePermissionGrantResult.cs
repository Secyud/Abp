using Volo.Abp;

namespace Secyud.Abp.Authorization.Permissions;

public class MultiplePermissionGrantResult
{
    public bool AllGranted
    {
        get { return Result.Values.All(x => x == PermissionGrantResult.Granted); }
    }

    public bool AllProhibited
    {
        get { return Result.Values.All(x => x == PermissionGrantResult.Prohibited); }
    }

    public Dictionary<string, PermissionGrantResult> Result { get; }

    public MultiplePermissionGrantResult()
    {
        Result = new Dictionary<string, PermissionGrantResult>();
    }

    public MultiplePermissionGrantResult(string[] names,
        PermissionGrantResult grantResult = PermissionGrantResult.Unset)
    {
        Check.NotNull(names, nameof(names));

        Result = new Dictionary<string, PermissionGrantResult>();

        foreach (var name in names)
        {
            Result.Add(name, grantResult);
        }
    }
}