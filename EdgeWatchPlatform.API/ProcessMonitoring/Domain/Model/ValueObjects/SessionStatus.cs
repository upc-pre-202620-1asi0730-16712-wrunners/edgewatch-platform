namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.ValueObjects;

/// <summary>Lifecycle of a spray session, exposed in lower case (active, completed, aborted, interrupted).</summary>
public enum SessionStatus { Active, Completed, Aborted, Interrupted }
