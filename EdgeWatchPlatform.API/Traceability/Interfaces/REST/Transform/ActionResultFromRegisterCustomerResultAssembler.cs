using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;
using EdgeWatchPlatform.API.Traceability.Application.Errors;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class ActionResultFromRegisterCustomerResultAssembler
{
    public static IActionResult ToActionResult(Result<Customer, RegisterCustomerError> result, IStringLocalizer localizer, HttpRequest request) =>
        result.Fold<IActionResult>(
            customer => new CreatedAtRouteResult("GetCustomerById", new { id = customer.Id }, CustomerResourceFromEntityAssembler.ToResourceFromEntity(customer)),
            error => error switch
            {
                RegisterCustomerError.DuplicateRuc => LocalizedProblemDetails.Create(localizer, StatusCodes.Status409Conflict, "ConflictTitle", "DuplicateCustomerRuc", request.Path),
                _ => LocalizedProblemDetails.Create(localizer, StatusCodes.Status500InternalServerError, "UnexpectedErrorTitle", "UnexpectedErrorDetail", request.Path)
            });
}
