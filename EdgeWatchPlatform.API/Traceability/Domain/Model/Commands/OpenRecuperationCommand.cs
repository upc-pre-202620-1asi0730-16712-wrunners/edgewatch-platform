using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;

public record OpenRecuperationCommand(int SupplierOrganizationId, WorkOrderNumber WorkOrderNumber, string ManufacturingOrderNumber,
    int ComponentId, int CustomerId, string Segment, string Operation, decimal WeightValue, string WeightUnitSymbol, int HourmeterAtEntry,
    string PowderSupplier, string PowderLotNumber, string PowderChemicalComposition, DateOnly ReceivedAt);
