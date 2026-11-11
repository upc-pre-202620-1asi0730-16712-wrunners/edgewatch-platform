namespace EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Resources;

public record ProcessReadingResource(long Id, int SpraySessionId, long EpochMillis, long PlcClockOffsetMillis, string TagPath, string Parameter,
    int? SubsystemId, int? PartId, decimal Value, string UnitSymbol, string UnitCategory, string Band, bool Derived, bool MappingPending);
