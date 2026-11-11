using EdgeWatchPlatform.API.Iam.Domain.Model;
using EdgeWatchPlatform.API.Iam.Domain.Model.Errors;
using EdgeWatchPlatform.API.Shared.Application.Model;
using EdgeWatchPlatform.API.Shared.Domain.Model.Errors;
using EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest.Transform;

/// <summary>Maps IAM results and errors to action results (problem details on failure).</summary>
public static class IamActionResultAssembler
{
    public static int ToStatusCodeFromIamError(IamError error) => error switch
    {
        IamError.UserNotFound => StatusCodes.Status404NotFound,
        IamError.OrganizationNotFound => StatusCodes.Status404NotFound,
        IamError.RoleNotFound => StatusCodes.Status404NotFound,
        IamError.EmailAlreadyTaken => StatusCodes.Status409Conflict,
        IamError.RucAlreadyTaken => StatusCodes.Status409Conflict,
        IamError.InvalidCredentials => StatusCodes.Status401Unauthorized,
        IamError.Unauthorized => StatusCodes.Status401Unauthorized,
        IamError.Forbidden => StatusCodes.Status403Forbidden,
        IamError.OperationCancelled => StatusCodes.Status409Conflict,
        IamError.DatabaseError => StatusCodes.Status500InternalServerError,
        IamError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    public static IActionResult ToActionResultFromResult<T>(ControllerBase controller, Result<T> result,
        ProblemDetailsFactory problemDetailsFactory, Func<T, IActionResult> successAction, params object[] detailArguments)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        var error = result.Error is IamError iamError ? iamError : IamError.InternalServerError;
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCodeFromIamError(error), error, result.DomainError, detailArguments);
    }

    public static IActionResult ToActionResultFromError(ControllerBase controller, IamError error, Error domainError,
        ProblemDetailsFactory problemDetailsFactory, params object[] detailArguments) =>
        problemDetailsFactory.CreateProblemDetails(controller, ToStatusCodeFromIamError(error), error, domainError, detailArguments);

    public static IActionResult ToActionResultFromGetUserResult<T>(ControllerBase controller, T? entity,
        ProblemDetailsFactory problemDetailsFactory, Func<T, IActionResult> successAction, object identifier) where T : class =>
        entity is null
            ? ToActionResultFromError(controller, IamError.UserNotFound, IamErrors.UserNotFound, problemDetailsFactory, identifier)
            : successAction(entity);
}
