namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

/// <summary>Representation of a recuperation.</summary>
public record RecuperationResource(int Id, string WorkOrderNumber, string ManufacturingOrderNumber, int ComponentId, int CustomerId,
    int SupplierOrganizationId, string Segment, string Operation, decimal WeightValue, string WeightUnitSymbol, int HourmeterAtEntry,
    string PowderSupplier, string PowderLotNumber, string PowderChemicalComposition, string Status, DateOnly ReceivedAt,
    DateOnly? CompletedAt, IEnumerable<LinkedSessionResource> LinkedSessions);
