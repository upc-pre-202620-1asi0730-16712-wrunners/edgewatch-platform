using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;
using EdgeWatchPlatform.API.Traceability.Application.Errors;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class ActionResultFromUpdateRecuperationResultAssembler
{
    public static IActionResult ToActionResult(Result<Recuperation, UpdateRecuperationError> result, IStringLocalizer localizer, HttpRequest request) =>
        result.Fold<IActionResult>(
            recuperation => new OkObjectResult(RecuperationResourceFromEntityAssembler.ToResourceFromEntity(recuperation)),
            error => error switch
            {
                UpdateRecuperationError.RecuperationNotFound => LocalizedProblemDetails.Create(localizer, StatusCodes.Status404NotFound, "NotFoundTitle", "RecuperationNotFound", request.Path),
                _ => LocalizedProblemDetails.Create(localizer, StatusCodes.Status500InternalServerError, "UnexpectedErrorTitle", "UnexpectedErrorDetail", request.Path)
            });
}
