using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;

/// <summary>Builds localized <see cref="ProblemDetails"/> responses for expected failures.</summary>
public static class LocalizedProblemDetails
{
    public static ObjectResult Create(IStringLocalizer localizer, int status, string titleKey, string detailKey, string? instance = null) =>
        Create(localizer, status, titleKey, detailKey, instance, []);

    /// <summary>Same as <see cref="Create(IStringLocalizer,int,string,string,string?)"/>, formatting the detail with <paramref name="detailArguments"/> ({0}, {1}...).</summary>
    public static ObjectResult Create(IStringLocalizer localizer, int status, string titleKey, string detailKey, string? instance, params object[] detailArguments)
    {
        var problem = new ProblemDetails
        {
            Status = status,
            Title = localizer[titleKey],
            Detail = localizer[detailKey, detailArguments],
            Instance = instance,
            Type = $"https://httpstatuses.io/{status}"
        };
        return new ObjectResult(problem) { StatusCode = status, ContentTypes = { "application/problem+json" } };
    }
}
