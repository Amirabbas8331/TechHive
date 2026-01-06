

namespace TechHive.Application.Common;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AuthorizeRolesAttribute : Attribute
{
    public string role { get; }

    public AuthorizeRolesAttribute(string role)
    {
        this.role = role;
    }
}
