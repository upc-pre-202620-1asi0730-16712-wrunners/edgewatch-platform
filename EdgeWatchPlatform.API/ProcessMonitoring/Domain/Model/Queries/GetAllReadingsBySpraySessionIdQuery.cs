using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Queries;

/// <summary>Readings of a session, optionally filtered by band.</summary>
public record GetAllReadingsBySpraySessionIdQuery(int SpraySessionId, Band? Band = null);
