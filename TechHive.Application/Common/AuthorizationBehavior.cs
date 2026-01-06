
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

        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            throw new UnauthorizedAccessException("No HTTP context available.");
        }


        if (!httpContext.User.Identity?.IsAuthenticated ?? false)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var requiredRole = authorizeAttribute.role;

        if (string.IsNullOrEmpty(requiredRole))
        {
            return await next();
        }

        if (!httpContext.User.IsInRole(requiredRole))
        {
            throw new ForbiddenAccessException("User does not have the required role.");
        }

        return await next();
    }
}
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string message) : base(message) { }
}