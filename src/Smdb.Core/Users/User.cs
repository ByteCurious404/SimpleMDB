namespace Smdb.Core.Users;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int BirthYear { get; set; }
    public string Bio { get; set; } = string.Empty;

    public User(int id, string username, string email, string fullName, int birthYear, string bio)
    {
        Id = id;
        Username = username;
        Email = email;
        FullName = fullName;
        BirthYear = birthYear;
        Bio = bio;
    }

    public override string ToString()
    {
        return $"User[Id={Id}, Username={Username}, Email={Email}, FullName={FullName}, BirthYear={BirthYear}]";
    }
}