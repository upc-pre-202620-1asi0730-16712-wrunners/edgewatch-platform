using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;
using EdgeWatchPlatform.API.Traceability.Application.Errors;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class ActionResultFromRegisterComponentResultAssembler
{
    public static IActionResult ToActionResult(Result<RecoveredComponent, RegisterComponentError> result, IStringLocalizer localizer, HttpRequest request) =>
        result.Fold<IActionResult>(
            component => new CreatedAtRouteResult("GetComponentById", new { id = component.Id }, ComponentResourceFromEntityAssembler.ToResourceFromEntity(component)),
            error => error switch
            {
                RegisterComponentError.CustomerNotFound => LocalizedProblemDetails.Create(localizer, StatusCodes.Status404NotFound, "NotFoundTitle", "CustomerNotFound", request.Path),
                RegisterComponentError.DuplicateSerialNumber => LocalizedProblemDetails.Create(localizer, StatusCodes.Status409Conflict, "ConflictTitle", "DuplicateComponentSerialNumber", request.Path),
                _ => LocalizedProblemDetails.Create(localizer, StatusCodes.Status500InternalServerError, "UnexpectedErrorTitle", "UnexpectedErrorDetail", request.Path)
            });
}
