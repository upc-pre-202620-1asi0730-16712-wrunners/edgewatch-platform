namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.ValueObjects;

/// <summary>Lifecycle of a fault case: open → acknowledged → resolved (exposed in lower case).</summary>
public enum FaultCaseStatus { Open, Acknowledged, Resolved }
