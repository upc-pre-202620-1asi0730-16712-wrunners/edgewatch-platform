using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;

public static class HvofSystemResourceFromEntityAssembler
{
    public static HvofSystemResource ToResourceFromEntity(HvofSystem entity) =>
        new(entity.Id, entity.Code.Value, entity.OrganizationId, entity.SerialNumber, entity.Status.ToUpperSnakeCase(),
            entity.SystemManufacturer, entity.SystemModel, entity.FuelType.ToUpperSnakeCase());
}
