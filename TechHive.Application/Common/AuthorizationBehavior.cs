
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace TechHive.Application.Common;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttribute = request.GetType()
            .GetCustomAttributes<AuthorizeRolesAttribute>(inherit: true)
            .FirstOrDefault();

        if (authorizeAttribute == null)
        {
            return await next();
        }

        var httpContext = _httpContextAccessor.HttpContext
            ?? throw new UnauthorizedAccessException("Accsess Denied");

        if (!httpContext.User.Identity?.IsAuthenticated ?? true)
        {
            throw new UnauthorizedAccessException("Not Authorized");
        }

        bool hasRequiredRole = authorizeAttribute.Roles
            .Any(requiredRole => httpContext.User.IsInRole(requiredRole));

        if (!hasRequiredRole)
        {
            throw new ForbiddenAccessException("UnauthorizedAccessException");
        }

        return await next();
    }
}
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string message) : base(message) { }
}