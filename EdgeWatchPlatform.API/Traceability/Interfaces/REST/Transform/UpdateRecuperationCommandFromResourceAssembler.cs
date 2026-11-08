using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class UpdateRecuperationCommandFromResourceAssembler
{
    public static UpdateRecuperationCommand ToCommandFromResource(int id, UpdateRecuperationResource resource) =>
        new(id, resource.ManufacturingOrderNumber,
            string.IsNullOrWhiteSpace(resource.Segment) ? RecuperationResourceDefaults.Segment : resource.Segment,
            resource.Operation, resource.WeightValue,
            string.IsNullOrWhiteSpace(resource.WeightUnitSymbol) ? RecuperationResourceDefaults.WeightUnitSymbol : resource.WeightUnitSymbol,
            resource.HourmeterAtEntry, resource.PowderSupplier, resource.PowderLotNumber, resource.PowderChemicalComposition);
}
