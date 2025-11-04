using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;

namespace Secyud.Abp.Identities;

public class ImportUsersFromFileInputWithStream
{
    public IRemoteStreamContent File { get; set; } = null!;
    
    [Range(1,2)]
    public ImportUsersFromFileType FileType { get; set; }
}
