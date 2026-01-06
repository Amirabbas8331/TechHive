

namespace TechHive.Application.Common;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AuthorizeRolesAttribute : Attribute
{
    public string[] Roles { get; }

    public AuthorizeRolesAttribute(params string[] roles)
    {
        Roles = roles;
    }
}
