using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;
using EdgeWatchPlatform.API.Traceability.Application.Errors;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class ActionResultFromUpdateCustomerResultAssembler
{
    public static IActionResult ToActionResult(Result<Customer, UpdateCustomerError> result, IStringLocalizer localizer, HttpRequest request) =>
        result.Fold<IActionResult>(
            customer => new OkObjectResult(CustomerResourceFromEntityAssembler.ToResourceFromEntity(customer)),
            error => error switch
            {
                UpdateCustomerError.CustomerNotFound => LocalizedProblemDetails.Create(localizer, StatusCodes.Status404NotFound, "NotFoundTitle", "CustomerNotFound", request.Path),
                _ => LocalizedProblemDetails.Create(localizer, StatusCodes.Status500InternalServerError, "UnexpectedErrorTitle", "UnexpectedErrorDetail", request.Path)
            });
}
