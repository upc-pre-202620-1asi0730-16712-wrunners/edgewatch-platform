namespace EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;

/// <summary>Links a process monitoring session to a recuperation (used by Process Monitoring when a session starts).</summary>
public record LinkSessionToRecuperationCommand(int RecuperationId, int SessionId);
