using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.Reporting.Domain.Model.Events;

public record ReportGeneratedEvent(int GeneratedReportId, int ReportTemplateId, int OrganizationId, string SubjectType, int SubjectId) : IEvent;
