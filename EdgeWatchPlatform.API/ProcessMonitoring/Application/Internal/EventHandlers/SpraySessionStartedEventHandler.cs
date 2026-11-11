using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Events;
using EdgeWatchPlatform.API.Shared.Application.Internal.EventHandlers;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Application.Internal.EventHandlers;

public class SpraySessionStartedEventHandler(ILogger<SpraySessionStartedEventHandler> logger) : IEventHandler<SpraySessionStartedEvent>
{
    public Task Handle(SpraySessionStartedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Spray session {SpraySessionId} started on HVOF system {HvofSystemId} for recuperation {RecuperationId} with recipe {RecipeNumber}",
            domainEvent.SpraySessionId, domainEvent.HvofSystemId, domainEvent.RecuperationId, domainEvent.RecipeNumber);
        return Task.CompletedTask;
    }
}
