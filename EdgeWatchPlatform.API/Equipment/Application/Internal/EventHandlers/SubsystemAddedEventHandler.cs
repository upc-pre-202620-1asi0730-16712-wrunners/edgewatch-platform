using EdgeWatchPlatform.API.Equipment.Domain.Model.Events;
using EdgeWatchPlatform.API.Shared.Application.Internal.EventHandlers;

namespace EdgeWatchPlatform.API.Equipment.Application.Internal.EventHandlers;

public class SubsystemAddedEventHandler(ILogger<SubsystemAddedEventHandler> logger) : IEventHandler<SubsystemAddedEvent>
{
    public Task Handle(SubsystemAddedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Subsystem {Alias} ({SubsystemType}) added to HVOF system {HvofSystemId}",
            domainEvent.Alias, domainEvent.SubsystemType, domainEvent.HvofSystemId);
        return Task.CompletedTask;
    }
}
