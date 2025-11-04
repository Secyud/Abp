namespace Secyud.Abp.Identities;

public class ImportUsersFromFileOutput
{
    public int AllCount { get; set; }

    public int SucceededCount { get; set; }

    public int FailedCount { get; set; }
    
    public string InvalidUsersDownloadToken { get; set; } = "";
    
    public bool IsAllSucceeded => AllCount == SucceededCount;
}