using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model;
using EdgeWatchPlatform.API.Shared.Application.Model;
using EdgeWatchPlatform.API.Shared.Domain.Model.Errors;
using EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Transform;

/// <summary>Maps Process Monitoring results and errors to action results (problem details on failure).</summary>
public static class ProcessMonitoringActionResultAssembler
{
    public static int ToStatusCodeFromProcessMonitoringError(ProcessMonitoringError error) => error switch
    {
        ProcessMonitoringError.SpraySessionNotFound => StatusCodes.Status404NotFound,
        ProcessMonitoringError.HvofSystemNotFound => StatusCodes.Status404NotFound,
        ProcessMonitoringError.RecuperationNotFound => StatusCodes.Status404NotFound,
        ProcessMonitoringError.ActiveRecipeNotFound => StatusCodes.Status422UnprocessableEntity,
        ProcessMonitoringError.RecuperationOfAnotherOrganization => StatusCodes.Status422UnprocessableEntity,
        ProcessMonitoringError.SessionNotActive => StatusCodes.Status422UnprocessableEntity,
        ProcessMonitoringError.InvalidSessionTransition => StatusCodes.Status422UnprocessableEntity,
        ProcessMonitoringError.InvalidReadings => StatusCodes.Status400BadRequest,
        ProcessMonitoringError.OperationCancelled => StatusCodes.Status409Conflict,
        ProcessMonitoringError.DatabaseError => StatusCodes.Status500InternalServerError,
        ProcessMonitoringError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    public static IActionResult ToActionResultFromResult<T>(ControllerBase controller, Result<T> result,
        ProblemDetailsFactory problemDetailsFactory, Func<T, IActionResult> successAction, params object[] detailArguments)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        var error = result.Error is ProcessMonitoringError pmError ? pmError : ProcessMonitoringError.InternalServerError;
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCodeFromProcessMonitoringError(error), error, result.DomainError, detailArguments);
    }

    public static IActionResult ToActionResultFromError(ControllerBase controller, ProcessMonitoringError error, Error domainError,
        ProblemDetailsFactory problemDetailsFactory, params object[] detailArguments) =>
        problemDetailsFactory.CreateProblemDetails(controller, ToStatusCodeFromProcessMonitoringError(error), error, domainError, detailArguments);
}
