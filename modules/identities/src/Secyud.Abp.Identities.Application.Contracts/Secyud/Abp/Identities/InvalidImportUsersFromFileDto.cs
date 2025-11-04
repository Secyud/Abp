namespace Secyud.Abp.Identities;

public class InvalidImportUsersFromFileDto : ImportUsersFromFileDto
{
    public string? ErrorReason { get; set; }
}