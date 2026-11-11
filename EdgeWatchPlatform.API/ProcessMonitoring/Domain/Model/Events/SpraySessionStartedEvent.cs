using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Events;

public record SpraySessionStartedEvent(int SpraySessionId, int HvofSystemId, int RecuperationId, int RecipeNumber) : IEvent;
