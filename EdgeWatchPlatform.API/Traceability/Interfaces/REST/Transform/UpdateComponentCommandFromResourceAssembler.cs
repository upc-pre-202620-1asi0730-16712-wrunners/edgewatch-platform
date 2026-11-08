using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class UpdateComponentCommandFromResourceAssembler
{
    public static UpdateComponentCommand ToCommandFromResource(int id, UpdateComponentResource resource) =>
        new(id, resource.PartNumber, resource.ComponentType.ToEnum<ComponentType>(), resource.MachineManufacturer,
            resource.MachineModel, resource.PcrTargetHours, resource.Status.ToEnum<ComponentStatus>());
}
