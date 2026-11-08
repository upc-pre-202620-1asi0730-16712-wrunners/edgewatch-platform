using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class OpenRecuperationCommandFromResourceAssembler
{
    public static OpenRecuperationCommand ToCommandFromResource(OpenRecuperationResource resource) =>
        new(resource.SupplierOrganizationId, new WorkOrderNumber(resource.WorkOrderNumber), resource.ManufacturingOrderNumber,
            resource.ComponentId, resource.CustomerId,
            string.IsNullOrWhiteSpace(resource.Segment) ? RecuperationResourceDefaults.Segment : resource.Segment,
            resource.Operation, resource.WeightValue,
            string.IsNullOrWhiteSpace(resource.WeightUnitSymbol) ? RecuperationResourceDefaults.WeightUnitSymbol : resource.WeightUnitSymbol,
            resource.HourmeterAtEntry, resource.PowderSupplier, resource.PowderLotNumber, resource.PowderChemicalComposition,
            resource.ReceivedAt ?? DateOnly.FromDateTime(DateTime.UtcNow));
}
