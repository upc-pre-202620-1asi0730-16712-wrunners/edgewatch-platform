using EdgeWatchPlatform.API.Equipment.Domain.Model;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Errors;
using EdgeWatchPlatform.API.Shared.Application.Model;
using EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;

/// <summary>Maps Equipment results and errors to action results (problem details on failure).</summary>
public static class EquipmentActionResultAssembler
{
    public static int ToStatusCodeFromEquipmentError(EquipmentError error) => error switch
    {
        EquipmentError.HvofSystemNotFound => StatusCodes.Status404NotFound,
        EquipmentError.DuplicateHvofSystemCode => StatusCodes.Status409Conflict,
        EquipmentError.OperationCancelled => StatusCodes.Status409Conflict,
        EquipmentError.DatabaseError => StatusCodes.Status500InternalServerError,
        EquipmentError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    public static IActionResult ToActionResultFromResult<T>(ControllerBase controller, Result<T> result,
        ProblemDetailsFactory problemDetailsFactory, Func<T, IActionResult> successAction, params object[] detailArguments)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        var error = result.Error is EquipmentError equipmentError ? equipmentError : EquipmentError.InternalServerError;
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCodeFromEquipmentError(error), error, result.DomainError, detailArguments);
    }

    public static IActionResult ToActionResultFromRegisterHvofSystemResult(ControllerBase controller, Result<HvofSystem> result,
        ProblemDetailsFactory problemDetailsFactory, Func<HvofSystem, IActionResult> successAction, string code) =>
        ToActionResultFromResult(controller, result, problemDetailsFactory, successAction, code);

    public static IActionResult ToActionResultFromGetHvofSystemByIdResult(ControllerBase controller, HvofSystem? hvofSystem,
        ProblemDetailsFactory problemDetailsFactory, Func<HvofSystem, IActionResult> successAction, int hvofSystemId)
    {
        if (hvofSystem is not null) return successAction(hvofSystem);
        return problemDetailsFactory.CreateProblemDetails(controller, StatusCodes.Status404NotFound, EquipmentError.HvofSystemNotFound,
            EquipmentErrors.HvofSystemNotFound, hvofSystemId);
    }
}
