using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Entities;

/// <summary>One telemetry sample of a spray session, already classified against the session recipe bands.</summary>
public class ProcessReading
{
    protected ProcessReading()
    {
        TagPath = string.Empty;
        Parameter = string.Empty;
        UnitSymbol = string.Empty;
        UnitCategory = string.Empty;
    }

    public ProcessReading(int spraySessionId, long epochMillis, long plcClockOffsetMillis, string tagPath, string parameter, int? subsystemId,
        int? partId, decimal value, string unitSymbol, string unitCategory, Band band, bool derived, bool mappingPending) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parameter);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(epochMillis);
        SpraySessionId = spraySessionId;
        EpochMillis = epochMillis;
        PlcClockOffsetMillis = plcClockOffsetMillis;
        TagPath = tagPath?.Trim() ?? string.Empty;
        Parameter = parameter.Trim();
        SubsystemId = subsystemId;
        PartId = partId;
        Value = value;
        UnitSymbol = unitSymbol?.Trim() ?? string.Empty;
        UnitCategory = unitCategory?.Trim() ?? string.Empty;
        Band = band;
        Derived = derived;
        MappingPending = mappingPending;
    }

    public long Id { get; private set; }
    public int SpraySessionId { get; private set; }
    public long EpochMillis { get; private set; }
    public long PlcClockOffsetMillis { get; private set; }
    public string TagPath { get; private set; }
    public string Parameter { get; private set; }
    public int? SubsystemId { get; private set; }
    public int? PartId { get; private set; }
    public decimal Value { get; private set; }
    public string UnitSymbol { get; private set; }
    public string UnitCategory { get; private set; }
    public Band Band { get; private set; }
    public bool Derived { get; private set; }
    public bool MappingPending { get; private set; }
}
