using EdgeWatchPlatform.API.Reporting.Domain.Model;
using EdgeWatchPlatform.API.Shared.Application.Model;
using EdgeWatchPlatform.API.Shared.Domain.Model.Errors;
using EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Transform;

public static class ReportingActionResultAssembler
{
    public static int ToStatusCodeFromReportingError(ReportingError error) => error switch
    {
        ReportingError.ReportTemplateNotFound => StatusCodes.Status404NotFound,
        ReportingError.GeneratedReportNotFound => StatusCodes.Status404NotFound,
        ReportingError.DuplicateReportTemplate => StatusCodes.Status409Conflict,
        ReportingError.ReportTemplateArchived => StatusCodes.Status422UnprocessableEntity,
        ReportingError.InvalidTemplateTransition => StatusCodes.Status422UnprocessableEntity,
        ReportingError.InvalidTemplateLayout => StatusCodes.Status400BadRequest,
        ReportingError.Forbidden => StatusCodes.Status403Forbidden,
        ReportingError.OperationCancelled => StatusCodes.Status409Conflict,
        ReportingError.DatabaseError => StatusCodes.Status500InternalServerError,
        ReportingError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    public static IActionResult ToActionResultFromResult<T>(ControllerBase controller, Result<T> result,
        ProblemDetailsFactory problemDetailsFactory, Func<T, IActionResult> successAction, params object[] detailArguments)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        var error = result.Error is ReportingError reportingError ? reportingError : ReportingError.InternalServerError;
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCodeFromReportingError(error), error, result.DomainError, detailArguments);
    }

    public static IActionResult ToActionResultFromError(ControllerBase controller, ReportingError error, Error domainError,
        ProblemDetailsFactory problemDetailsFactory, params object[] detailArguments) =>
        problemDetailsFactory.CreateProblemDetails(controller, ToStatusCodeFromReportingError(error), error, domainError, detailArguments);
}
