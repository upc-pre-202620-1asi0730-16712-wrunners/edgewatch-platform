using EdgeWatchPlatform.API.Equipment.Application.Errors;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;

public static class ActionResultFromRegisterHvofSystemResultAssembler
{
    public static IActionResult ToActionResult(Result<HvofSystem, RegisterHvofSystemError> result, IStringLocalizer localizer, HttpRequest request) =>
        result.Fold<IActionResult>(
            system => new CreatedAtRouteResult("GetHvofSystemById", new { id = system.Id }, HvofSystemResourceFromEntityAssembler.ToResourceFromEntity(system)),
            error => error switch
            {
                RegisterHvofSystemError.DuplicateCode => LocalizedProblemDetails.Create(localizer, StatusCodes.Status409Conflict, "ConflictTitle", "DuplicateHvofSystemCode", request.Path),
                RegisterHvofSystemError.InvalidCode => LocalizedProblemDetails.Create(localizer, StatusCodes.Status400BadRequest, "ValidationErrorTitle", "InvalidCode", request.Path),
                _ => LocalizedProblemDetails.Create(localizer, StatusCodes.Status500InternalServerError, "UnexpectedErrorTitle", "UnexpectedErrorDetail", request.Path)
            });
}
