using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Identities;

public class GetIdentityUserListAsFileInput : GetIdentityUsersInput
{
    [Required]
    public string Token { get; set; } = string.Empty;
}