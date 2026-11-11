using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.Reporting.Domain.Model.Events;

public record ReportTemplateCreatedEvent(int ReportTemplateId, int OrganizationId, string ReportKind) : IEvent;
