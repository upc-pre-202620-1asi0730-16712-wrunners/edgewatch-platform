namespace EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Resources;

public record GeneratedReportResource(int Id, int ReportTemplateId, int OrganizationId, int GeneratedByUserId, string SubjectType, int SubjectId,
    string Format, string FileName, DateOnly? PeriodFrom, DateOnly? PeriodTo, DateTimeOffset GeneratedAt);
