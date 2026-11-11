using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Entities;
using EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Transform;

public static class ProcessReadingResourceFromEntityAssembler
{
    public static ProcessReadingResource ToResourceFromEntity(ProcessReading reading) =>
        new(reading.Id, reading.SpraySessionId, reading.EpochMillis, reading.PlcClockOffsetMillis, reading.TagPath, reading.Parameter, reading.SubsystemId,
            reading.PartId, reading.Value, reading.UnitSymbol, reading.UnitCategory, reading.Band.ToUpperSnakeCase().ToLowerInvariant(), reading.Derived,
            reading.MappingPending);
}
