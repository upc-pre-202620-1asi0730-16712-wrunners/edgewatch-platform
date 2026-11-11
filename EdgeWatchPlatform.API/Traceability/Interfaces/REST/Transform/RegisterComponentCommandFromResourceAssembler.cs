using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class RegisterComponentCommandFromResourceAssembler
{
    public static RegisterComponentCommand ToCommandFromResource(RegisterComponentResource resource) =>
        new(resource.CustomerId, resource.SerialNumber, resource.PartNumber, resource.ComponentType.ToEnum<ComponentType>(),
            resource.MachineManufacturer, resource.MachineModel, resource.PcrTargetHours);
}
