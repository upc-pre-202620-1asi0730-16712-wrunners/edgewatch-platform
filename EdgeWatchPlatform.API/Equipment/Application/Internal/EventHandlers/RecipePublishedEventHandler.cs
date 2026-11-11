using EdgeWatchPlatform.API.Equipment.Domain.Model.Events;
using EdgeWatchPlatform.API.Shared.Application.Internal.EventHandlers;

namespace EdgeWatchPlatform.API.Equipment.Application.Internal.EventHandlers;

public class RecipePublishedEventHandler(ILogger<RecipePublishedEventHandler> logger) : IEventHandler<RecipePublishedEvent>
{
    public Task Handle(RecipePublishedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Recipe {RecipeNumber} ({Name}) published for HVOF system {HvofSystemId}",
            domainEvent.RecipeNumber, domainEvent.Name, domainEvent.HvofSystemId);
        return Task.CompletedTask;
    }
}
