using EdgeWatchPlatform.API.Shared.Resources;
using EdgeWatchPlatform.API.Shared.Resources.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Shared.Infrastructure.Pipeline.Middleware.Components;

/// <summary>Writes RFC 9457 problem details for unhandled exceptions (409 on cancellation, 500 otherwise).</summary>
public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    IStringLocalizer<CommonMessages> commonLocalizer) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails;
        if (exception is OperationCanceledException)
        {
            logger.LogWarning(exception, "Request was cancelled: {Message}", exception.Message);
            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = errorLocalizer["OperationCancelled"],
                Detail = errorLocalizer["OperationCancelled"],
                Instance = httpContext.Request.Path
            };
        }
        else
        {
            logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = commonLocalizer["InternalServerError"],
                Detail = errorLocalizer["GenericError"],
                Instance = httpContext.Request.Path
            };
        }

        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception
        });
    }
}
