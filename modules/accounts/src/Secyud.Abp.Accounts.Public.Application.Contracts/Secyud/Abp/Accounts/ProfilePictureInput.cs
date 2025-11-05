using Volo.Abp.Content;

namespace Secyud.Abp.Accounts;

public class ProfilePictureInput
{
    public ProfilePictureType Type { get; set; }

    public required IRemoteStreamContent ImageContent { get; set; }
}
