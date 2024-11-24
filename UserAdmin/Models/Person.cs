using System.Collections.Generic;

namespace UserAdmin.Models;

public class Person(
    string firstName,
    string lastName,
    string login,
    string password,
    string email,
    string accessLevel,
    string userNotes)
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Login { get; set; } = login;
    public string Password { get; set; } = password;
    public string Email { get; set; } = email;
    public List<string> AccessLevels { get; set; } = ["Admin", "Moderator", "Guest", "User"];
    public string AccessLevel { get; set; } = accessLevel;
    public string UserNotes { get; set; } = userNotes;
}