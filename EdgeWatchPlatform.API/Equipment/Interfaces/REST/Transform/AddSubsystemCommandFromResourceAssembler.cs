using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;

public static class AddSubsystemCommandFromResourceAssembler
{
    public static AddSubsystemToHvofSystemCommand ToCommandFromResource(int hvofSystemId, AddSubsystemResource resource) =>
        new(hvofSystemId, resource.SubsystemType.ToEnum<SubsystemType>(), resource.Name, resource.Alias,
            (resource.Parameters ?? []).Select(p => new ParameterName(p.Parameter)).ToList());
}
