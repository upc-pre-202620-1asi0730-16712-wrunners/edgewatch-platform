using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Events;

public record SubsystemAddedEvent(int HvofSystemId, string Alias, string SubsystemType) : IEvent;
