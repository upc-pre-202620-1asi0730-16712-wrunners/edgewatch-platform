using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model;
using EdgeWatchPlatform.API.Shared.Application.Model;
using EdgeWatchPlatform.API.Shared.Domain.Model.Errors;
using EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Transform;

public static class FaultDiagnosisActionResultAssembler
{
    public static int ToStatusCodeFromFaultDiagnosisError(FaultDiagnosisError error) => error switch
    {
        FaultDiagnosisError.FaultRuleNotFound => StatusCodes.Status404NotFound,
        FaultDiagnosisError.FaultCaseNotFound => StatusCodes.Status404NotFound,
        FaultDiagnosisError.DuplicateFaultRule => StatusCodes.Status409Conflict,
        FaultDiagnosisError.InvalidFaultCaseTransition => StatusCodes.Status422UnprocessableEntity,
        FaultDiagnosisError.Forbidden => StatusCodes.Status403Forbidden,
        FaultDiagnosisError.OperationCancelled => StatusCodes.Status409Conflict,
        FaultDiagnosisError.DatabaseError => StatusCodes.Status500InternalServerError,
        FaultDiagnosisError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    public static IActionResult ToActionResultFromResult<T>(ControllerBase controller, Result<T> result,
        ProblemDetailsFactory problemDetailsFactory, Func<T, IActionResult> successAction, params object[] detailArguments)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        var error = result.Error is FaultDiagnosisError fdError ? fdError : FaultDiagnosisError.InternalServerError;
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCodeFromFaultDiagnosisError(error), error, result.DomainError, detailArguments);
    }

    public static IActionResult ToActionResultFromError(ControllerBase controller, FaultDiagnosisError error, Error domainError,
        ProblemDetailsFactory problemDetailsFactory, params object[] detailArguments) =>
        problemDetailsFactory.CreateProblemDetails(controller, ToStatusCodeFromFaultDiagnosisError(error), error, domainError, detailArguments);
}
