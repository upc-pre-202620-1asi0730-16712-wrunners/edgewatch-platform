using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;

/// <summary>Built by the out-of-band reading event handler, never by a controller.</summary>
public record OpenFaultCaseCommand(int OrganizationId, int SpraySessionId, int HvofSystemId, string Parameter, FaultBand Band, decimal Value, long EpochMillis);
