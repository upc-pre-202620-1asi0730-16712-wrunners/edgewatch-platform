using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Events;

/// <summary>Raised for every warning or shutdown reading; Fault Diagnosis opens fault cases from it.</summary>
public record OutOfBandReadingDetectedEvent(int SpraySessionId, int HvofSystemId, int OrganizationId, string Parameter, string Band,
    decimal Value, long EpochMillis, string UnitSymbol) : IEvent;
