namespace EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;

public record UpdateRecuperationCommand(int RecuperationId, string ManufacturingOrderNumber, string Segment, string Operation,
    decimal WeightValue, string WeightUnitSymbol, int HourmeterAtEntry, string PowderSupplier, string PowderLotNumber,
    string PowderChemicalComposition);
