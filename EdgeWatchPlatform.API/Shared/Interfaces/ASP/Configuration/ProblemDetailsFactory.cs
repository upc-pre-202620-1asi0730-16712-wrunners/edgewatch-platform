using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;

/// <summary>Builds localized <see cref="ProblemDetails"/> responses for expected failures.</summary>
public static class LocalizedProblemDetails
{
    public static ObjectResult Create(IStringLocalizer localizer, int status, string titleKey, string detailKey, string? instance = null)
    {
        var problem = new ProblemDetails
        {
            Status = status,
            Title = localizer[titleKey],
            Detail = localizer[detailKey],
            Instance = instance,
            Type = $"https://httpstatuses.io/{status}"
        };
        return new ObjectResult(problem) { StatusCode = status, ContentTypes = { "application/problem+json" } };
    }
}
