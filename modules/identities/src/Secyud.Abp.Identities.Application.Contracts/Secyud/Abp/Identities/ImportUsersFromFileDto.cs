namespace Secyud.Abp.Identities;

public class ImportUsersFromFileDto
{
    public string? UserName { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? EmailAddress { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Password { get; set; }

    public string? AssignedRoleNames { get; set; }

    public override string? ToString()
    {
        return $"UserName: {UserName}, Name: {Name}, Surname: {Surname}, EmailAddress: {EmailAddress}, PhoneNumber: {PhoneNumber}, Password: {Password}, AssignedRoleNames: {AssignedRoleNames}";
    }
}