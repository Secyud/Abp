namespace Secyud.Abp.Accounts;

public class ProfilePictureSourceDto
{
    public ProfilePictureType Type { get; set; }

    public string Source { get; set; }= "";

    public required byte[] FileContent { get; set; }
}
