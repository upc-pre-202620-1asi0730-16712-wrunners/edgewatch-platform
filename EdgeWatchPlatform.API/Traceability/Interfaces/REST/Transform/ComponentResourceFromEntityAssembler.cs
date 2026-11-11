using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class ComponentResourceFromEntityAssembler
{
    public static ComponentResource ToResourceFromEntity(RecoveredComponent entity) =>
        new(entity.Id, entity.SerialNumber, entity.PartNumber, entity.ComponentType.ToUpperSnakeCase(), entity.MachineManufacturer,
            entity.MachineModel, entity.CustomerId, entity.PcrTargetHours, entity.Status.ToUpperSnakeCase());
}
