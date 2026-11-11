namespace EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Resources;

public record FaultCaseResource(int Id, int OrganizationId, int SpraySessionId, int HvofSystemId, string Parameter, string Band, decimal Value, long EpochMillis,
    string ProbableCause, string RecommendedAction, string Status, int? AcknowledgedBy, DateTimeOffset? AcknowledgedAt, int? ResolvedBy, DateTimeOffset? ResolvedAt,
    string? ResolutionNotes);
