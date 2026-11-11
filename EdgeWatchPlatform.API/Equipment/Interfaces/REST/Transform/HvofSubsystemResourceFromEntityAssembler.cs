using EdgeWatchPlatform.API.Equipment.Domain.Model.Entities;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;

public static class HvofSubsystemResourceFromEntityAssembler
{
    public static HvofSubsystemResource ToResourceFromEntity(HvofSubsystem subsystem) =>
        new(subsystem.Id, subsystem.HvofSystemId, subsystem.SubsystemType.ToUpperSnakeCase(), subsystem.Name, subsystem.Alias,
            subsystem.Parameters.Select(p => new ParameterNameResource(p.Parameter)).ToList(),
            subsystem.Parts.Select(HvofPartResourceFromEntityAssembler.ToResourceFromEntity).ToList());
}
