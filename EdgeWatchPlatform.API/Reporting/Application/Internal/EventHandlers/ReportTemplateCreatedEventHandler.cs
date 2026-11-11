using EdgeWatchPlatform.API.Reporting.Domain.Model.Events;
using EdgeWatchPlatform.API.Shared.Application.Internal.EventHandlers;

namespace EdgeWatchPlatform.API.Reporting.Application.Internal.EventHandlers;

public class ReportTemplateCreatedEventHandler(ILogger<ReportTemplateCreatedEventHandler> logger) : IEventHandler<ReportTemplateCreatedEvent>
{
    public Task Handle(ReportTemplateCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Report template {ReportTemplateId} ({ReportKind}) created for organization {OrganizationId}",
            domainEvent.ReportTemplateId, domainEvent.ReportKind, domainEvent.OrganizationId);
        return Task.CompletedTask;
    }
}
