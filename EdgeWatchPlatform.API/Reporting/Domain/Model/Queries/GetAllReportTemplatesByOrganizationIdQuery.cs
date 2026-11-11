using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Reporting.Domain.Model.Queries;

public record GetAllReportTemplatesByOrganizationIdQuery(int OrganizationId, ReportKind? ReportKind);
