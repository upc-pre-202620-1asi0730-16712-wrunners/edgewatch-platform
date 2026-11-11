namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Commands;

public record ProcessReadingItem(long EpochMillis, long PlcClockOffsetMillis, string TagPath, string Parameter, int? SubsystemId, int? PartId,
    decimal Value, string? UnitSymbol, string? UnitCategory, bool Derived, bool MappingPending);

/// <summary>Stores a batch of readings of a session, classifying each one against the session recipe bands.</summary>
public record IngestProcessReadingsCommand(int SpraySessionId, IReadOnlyCollection<ProcessReadingItem> Readings);
