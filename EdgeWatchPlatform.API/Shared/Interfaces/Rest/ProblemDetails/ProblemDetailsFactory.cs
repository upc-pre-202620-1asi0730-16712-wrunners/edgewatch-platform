using EdgeWatchPlatform.API.Shared.Domain.Model.Errors;
using EdgeWatchPlatform.API.Shared.Resources;
using EdgeWatchPlatform.API.Shared.Resources.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;

/// <summary>
///     Builds RFC 9457 problem details responses from domain errors.
///     The title is localized by <c>&lt;ErrorEnumType&gt;.&lt;Member&gt;</c> and the detail by the <see cref="Error.Code"/>;
///     when a key has no translation the default message of the error is used.
/// </summary>
public class ProblemDetailsFactory(
    IStringLocalizer<ErrorMessages> errorLocalizer,
    IStringLocalizer<CommonMessages> commonLocalizer,
    Microsoft.AspNetCore.Mvc.Infrastructure.ProblemDetailsFactory aspNetCoreProblemDetailsFactory)
{
    /// <summary>Creates a problem details response for a domain error and its bounded context error enum.</summary>
    public IActionResult CreateProblemDetails(ControllerBase controller, int statusCode, Enum? errorEnum, Error error, params object[] detailArguments)
    {
        var title = ResolveTitle(errorEnum, error.Type);
        var detailResource = errorLocalizer[error.Code, detailArguments];
        var detail = detailResource.ResourceNotFound ? string.Format(error.Message, detailArguments) : detailResource.Value;
        return Create(controller, statusCode, title, detail);
    }

    /// <summary>Creates a problem details response for a bounded context error enum and an already localized detail.</summary>
    public IActionResult CreateProblemDetails(ControllerBase controller, int statusCode, Enum? errorEnum, string detailMessage)
    {
        return Create(controller, statusCode, ResolveTitle(errorEnum, null), detailMessage);
    }

    /// <summary>Creates a problem details response from a title key of the common messages and a detail key of the error messages.</summary>
    public IActionResult CreateProblemDetails(ControllerBase controller, int statusCode, string titleKey, string detailKey, params object[] detailArgs)
    {
        return Create(controller, statusCode, commonLocalizer[titleKey], errorLocalizer[detailKey, detailArgs]);
    }

    private string ResolveTitle(Enum? errorEnum, EErrorType? type)
    {
        if (errorEnum is not null)
        {
            var localized = errorLocalizer[$"{errorEnum.GetType().Name}.{errorEnum}"];
            if (!localized.ResourceNotFound) return localized.Value;
        }

        return type switch
        {
            EErrorType.Validation => commonLocalizer["ValidationError"],
            EErrorType.NotFound => commonLocalizer["NotFound"],
            EErrorType.Conflict => commonLocalizer["Conflict"],
            EErrorType.Unauthorized => commonLocalizer["Unauthorized"],
            EErrorType.Forbidden => commonLocalizer["Forbidden"],
            EErrorType.Failure => commonLocalizer["UnprocessableEntity"],
            EErrorType.Unexpected => commonLocalizer["InternalServerError"],
            _ => commonLocalizer["GenericError"]
        };
    }

    private IActionResult Create(ControllerBase controller, int statusCode, string title, string detail)
    {
        var problemDetails = aspNetCoreProblemDetailsFactory.CreateProblemDetails(controller.HttpContext, statusCode, title,
            detail: detail, instance: controller.HttpContext.Request.Path);
        return controller.StatusCode(statusCode, problemDetails);
    }
}
