using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Components;

namespace EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Extensions;

public static class RequestAuthorizationMiddlewareExtensions
{
    /// <summary>Registers <see cref="RequestAuthorizationMiddleware"/> in the pipeline.</summary>
    public static IApplicationBuilder UseRequestAuthorization(this IApplicationBuilder builder) =>
        builder.UseMiddleware<RequestAuthorizationMiddleware>();
}
