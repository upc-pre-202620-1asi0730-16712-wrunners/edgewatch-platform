using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Events;

public record ControllerAddedEvent(int HvofSystemId, int ControllerNumber, string IpAddress) : IEvent;
