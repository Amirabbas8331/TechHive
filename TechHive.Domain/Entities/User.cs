using Microsoft.AspNetCore.Identity;

namespace TechHive.Model;

public class User:IdentityUser
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; }
    public bool EmailVerified { get; set; }

}