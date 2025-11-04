using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Identities;

public class GetImportUsersSampleFileInput
{
    [Range(1,2)]
    public ImportUsersFromFileType FileType { get; set; }

    [Required]
    public string Token { get; set; } = string.Empty;
}