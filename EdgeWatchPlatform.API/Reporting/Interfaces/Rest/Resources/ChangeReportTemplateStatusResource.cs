using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Resources;

/// <summary><c>ACTIVE</c> or <c>ARCHIVED</c>.</summary>
public record ChangeReportTemplateStatusResource(
    [Required(ErrorMessage = "RequiredStatus")] [RegularExpression("^(ACTIVE|ARCHIVED)$", ErrorMessage = "InvalidTemplateStatus")] string Status);
