using EdgeWatchPlatform.API.Iam.Application.Internal.OutboundServices;
using EdgeWatchPlatform.API.Iam.Application.QueryServices;
using EdgeWatchPlatform.API.Iam.Domain.Model.Queries;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;

namespace EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
///     Reads the bearer token of the request, validates it and loads the user into <c>HttpContext.Items["User"]</c>.
///     Endpoints marked with <see cref="AllowAnonymousAttribute"/> are skipped.
/// </summary>
public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUserQueryService userQueryService, ITokenService tokenService)
    {
        var allowAnonymous = context.GetEndpoint()?.Metadata.Any(m => m.GetType() == typeof(AllowAnonymousAttribute)) ?? false;
        if (allowAnonymous)
        {
            await next(context);
            return;
        }

        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        if (!string.IsNullOrEmpty(token))
        {
            var userId = await tokenService.ValidateToken(token);
            if (userId is not null)
            {
                var user = await userQueryService.Handle(new GetUserByIdQuery(userId.Value), context.RequestAborted);
                if (user is not null) context.Items["User"] = user;
            }
        }

        await next(context);
    }
}
