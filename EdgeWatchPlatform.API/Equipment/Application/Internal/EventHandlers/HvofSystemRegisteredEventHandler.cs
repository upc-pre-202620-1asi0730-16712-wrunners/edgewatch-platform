using EdgeWatchPlatform.API.Equipment.Domain.Model.Events;
using EdgeWatchPlatform.API.Shared.Application.Internal.EventHandlers;

namespace EdgeWatchPlatform.API.Equipment.Application.Internal.EventHandlers;

/// <summary>Logs every registered HVOF system (other contexts react to the same event without coupling).</summary>
public class HvofSystemRegisteredEventHandler(ILogger<HvofSystemRegisteredEventHandler> logger) : IEventHandler<HvofSystemRegisteredEvent>
{
    public Task Handle(HvofSystemRegisteredEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("HVOF system {Code} (serial {SerialNumber}) registered for organization {OrganizationId}",
            domainEvent.Code, domainEvent.SerialNumber, domainEvent.OrganizationId);
        return Task.CompletedTask;
    }
}
