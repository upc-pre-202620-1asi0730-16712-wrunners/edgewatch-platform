using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;
using EdgeWatchPlatform.API.Traceability.Application.Errors;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class ActionResultFromOpenRecuperationResultAssembler
{
    public static IActionResult ToActionResult(Result<Recuperation, OpenRecuperationError> result, IStringLocalizer localizer, HttpRequest request) =>
        result.Fold<IActionResult>(
            recuperation => new CreatedAtRouteResult("GetRecuperationById", new { id = recuperation.Id }, RecuperationResourceFromEntityAssembler.ToResourceFromEntity(recuperation)),
            error => error switch
            {
                OpenRecuperationError.CustomerNotFound => LocalizedProblemDetails.Create(localizer, StatusCodes.Status404NotFound, "NotFoundTitle", "CustomerNotFound", request.Path),
                OpenRecuperationError.ComponentNotFound => LocalizedProblemDetails.Create(localizer, StatusCodes.Status404NotFound, "NotFoundTitle", "ComponentNotFound", request.Path),
                OpenRecuperationError.DuplicateWorkOrderNumber => LocalizedProblemDetails.Create(localizer, StatusCodes.Status409Conflict, "ConflictTitle", "DuplicateWorkOrderNumber", request.Path),
                OpenRecuperationError.ComponentNotOfCustomer => LocalizedProblemDetails.Create(localizer, StatusCodes.Status422UnprocessableEntity, "UnprocessableTitle", "ComponentNotOfCustomer", request.Path),
                _ => LocalizedProblemDetails.Create(localizer, StatusCodes.Status500InternalServerError, "UnexpectedErrorTitle", "UnexpectedErrorDetail", request.Path)
            });
}
