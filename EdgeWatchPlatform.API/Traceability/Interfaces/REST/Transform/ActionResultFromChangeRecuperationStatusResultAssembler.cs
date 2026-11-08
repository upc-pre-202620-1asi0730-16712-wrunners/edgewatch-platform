using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;
using EdgeWatchPlatform.API.Traceability.Application.Errors;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class ActionResultFromChangeRecuperationStatusResultAssembler
{
    /// <summary>Maps the status change result to 200, 404 or 409 (explaining the rejected transition from <paramref name="currentStatus"/> to <paramref name="requestedStatus"/>).</summary>
    public static IActionResult ToActionResult(Result<Recuperation, ChangeRecuperationStatusError> result, RecuperationStatus? currentStatus,
        RecuperationStatus requestedStatus, IStringLocalizer localizer, HttpRequest request) =>
        result.Fold<IActionResult>(
            recuperation => new OkObjectResult(RecuperationResourceFromEntityAssembler.ToResourceFromEntity(recuperation)),
            error => error switch
            {
                ChangeRecuperationStatusError.RecuperationNotFound => LocalizedProblemDetails.Create(localizer, StatusCodes.Status404NotFound, "NotFoundTitle", "RecuperationNotFound", request.Path),
                ChangeRecuperationStatusError.InvalidTransition => LocalizedProblemDetails.Create(localizer, StatusCodes.Status409Conflict, "ConflictTitle", "InvalidRecuperationTransition", request.Path,
                    currentStatus?.ToUpperSnakeCase() ?? string.Empty, requestedStatus.ToUpperSnakeCase()),
                _ => LocalizedProblemDetails.Create(localizer, StatusCodes.Status500InternalServerError, "UnexpectedErrorTitle", "UnexpectedErrorDetail", request.Path)
            });
}
