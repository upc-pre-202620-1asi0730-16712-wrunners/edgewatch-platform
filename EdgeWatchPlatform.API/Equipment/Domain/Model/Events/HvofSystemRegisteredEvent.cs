using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Events;

/// <summary>Raised when an HVOF system is registered for an organization.</summary>
public record HvofSystemRegisteredEvent(int OrganizationId, string Code, string SerialNumber) : IEvent;
