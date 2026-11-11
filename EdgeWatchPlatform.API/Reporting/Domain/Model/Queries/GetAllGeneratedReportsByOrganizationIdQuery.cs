using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Reporting.Domain.Model.Queries;

public record GetAllGeneratedReportsByOrganizationIdQuery(int OrganizationId, ReportSubjectType? SubjectType, int? SubjectId);
