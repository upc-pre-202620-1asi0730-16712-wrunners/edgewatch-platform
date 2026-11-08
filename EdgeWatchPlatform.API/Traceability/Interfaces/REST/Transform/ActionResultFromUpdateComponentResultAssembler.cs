using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;
using EdgeWatchPlatform.API.Traceability.Application.Errors;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class ActionResultFromUpdateComponentResultAssembler
{
    public static IActionResult ToActionResult(Result<RecoveredComponent, UpdateComponentError> result, IStringLocalizer localizer, HttpRequest request) =>
        result.Fold<IActionResult>(
            component => new OkObjectResult(ComponentResourceFromEntityAssembler.ToResourceFromEntity(component)),
            error => error switch
            {
                UpdateComponentError.ComponentNotFound => LocalizedProblemDetails.Create(localizer, StatusCodes.Status404NotFound, "NotFoundTitle", "ComponentNotFound", request.Path),
                _ => LocalizedProblemDetails.Create(localizer, StatusCodes.Status500InternalServerError, "UnexpectedErrorTitle", "UnexpectedErrorDetail", request.Path)
            });
}
