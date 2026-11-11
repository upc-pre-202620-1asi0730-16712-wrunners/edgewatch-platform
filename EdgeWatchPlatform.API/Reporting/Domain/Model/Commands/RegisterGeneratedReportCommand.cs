using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;

/// <summary>Records that a report was rendered from a template for a subject (the file itself stays with the user).</summary>
public record RegisterGeneratedReportCommand(int ReportTemplateId, int OrganizationId, int GeneratedByUserId, ReportSubjectType SubjectType,
    int SubjectId, string FileName, DateOnly? PeriodFrom, DateOnly? PeriodTo);
