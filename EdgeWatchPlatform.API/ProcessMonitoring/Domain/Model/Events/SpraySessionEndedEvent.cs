using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Events;

public record SpraySessionEndedEvent(int SpraySessionId, int RecuperationId, string Status) : IEvent;
