using EdgeWatchPlatform.API.Reporting.Domain.Model.Events;
using EdgeWatchPlatform.API.Shared.Application.Internal.EventHandlers;

namespace EdgeWatchPlatform.API.Reporting.Application.Internal.EventHandlers;

public class ReportGeneratedEventHandler(ILogger<ReportGeneratedEventHandler> logger) : IEventHandler<ReportGeneratedEvent>
{
    public Task Handle(ReportGeneratedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Report {GeneratedReportId} generated from template {ReportTemplateId} for {SubjectType} {SubjectId}",
            domainEvent.GeneratedReportId, domainEvent.ReportTemplateId, domainEvent.SubjectType, domainEvent.SubjectId);
        return Task.CompletedTask;
    }
}
