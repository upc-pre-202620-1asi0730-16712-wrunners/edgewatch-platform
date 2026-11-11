using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;

public static class AddPartCommandFromResourceAssembler
{
    public static AddPartToSubsystemCommand ToCommandFromResource(int hvofSystemId, int subsystemId, AddPartResource resource) =>
        new(hvofSystemId, subsystemId, resource.PartType.ToEnum<PartType>(), resource.SerialNumber, resource.Manufacturer);
}
