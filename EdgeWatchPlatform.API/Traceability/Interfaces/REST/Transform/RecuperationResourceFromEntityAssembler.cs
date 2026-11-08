using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class RecuperationResourceFromEntityAssembler
{
    public static RecuperationResource ToResourceFromEntity(Recuperation entity) =>
        new(entity.Id, entity.WorkOrderNumber.Value, entity.ManufacturingOrderNumber, entity.ComponentId, entity.CustomerId,
            entity.SupplierOrganizationId, entity.Segment, entity.Operation, entity.WeightValue, entity.WeightUnitSymbol,
            entity.HourmeterAtEntry, entity.PowderSupplier, entity.PowderLotNumber, entity.PowderChemicalComposition,
            entity.Status.ToUpperSnakeCase(), entity.ReceivedAt, entity.CompletedAt,
            entity.LinkedSessions.Select(s => new LinkedSessionResource(s.SessionId)).ToList());
}
