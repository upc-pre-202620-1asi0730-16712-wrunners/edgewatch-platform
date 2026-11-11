using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Commands;

public record AbortSpraySessionCommand(int SpraySessionId, AbortReason Reason);
