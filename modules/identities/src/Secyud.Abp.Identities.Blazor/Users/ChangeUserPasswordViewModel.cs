using System.ComponentModel.DataAnnotations;

namespace Secyud.Abp.Identities.Users;

public class ChangeUserPasswordViewModel
{
    public Guid Id { get; set; }

    public string? UserName { get; set; } 

    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = "";
}