using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Commands;
using EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Resources;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Transform;

public static class IngestProcessReadingsCommandFromResourceAssembler
{
    public static IngestProcessReadingsCommand ToCommandFromResource(int spraySessionId, IEnumerable<ProcessReadingItemResource> resources) =>
        new(spraySessionId, resources.Select(r => new ProcessReadingItem(r.EpochMillis, r.PlcClockOffsetMillis, r.TagPath ?? string.Empty, r.Parameter,
            r.SubsystemId, r.PartId, r.Value, r.UnitSymbol, r.UnitCategory, r.Derived, r.MappingPending)).ToList());
}
