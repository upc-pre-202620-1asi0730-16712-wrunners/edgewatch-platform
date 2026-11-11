using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;

/// <summary>Requires a signed-in user (set in <c>HttpContext.Items["User"]</c> by the authorization middleware); answers 401 otherwise.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous) return;
        var user = (User?)context.HttpContext.Items["User"];
        if (user is null) context.Result = new UnauthorizedResult();
    }
}
