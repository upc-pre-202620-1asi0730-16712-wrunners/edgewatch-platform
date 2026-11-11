using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Resources;

/// <summary>Request body of POST /generated-reports: a report was rendered from a template for a subject.</summary>
public record RegisterGeneratedReportResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidReportTemplateId")] int ReportTemplateId,
    [Required(ErrorMessage = "RequiredSubjectType")] [RegularExpression("^(SPRAY_SESSION|RECUPERATION)$", ErrorMessage = "InvalidSubjectType")] string SubjectType,
    [Range(1, int.MaxValue, ErrorMessage = "InvalidSubjectId")] int SubjectId,
    [Required(ErrorMessage = "RequiredFileName")] [MaxLength(160)] string FileName,
    DateOnly? PeriodFrom,
    DateOnly? PeriodTo);
