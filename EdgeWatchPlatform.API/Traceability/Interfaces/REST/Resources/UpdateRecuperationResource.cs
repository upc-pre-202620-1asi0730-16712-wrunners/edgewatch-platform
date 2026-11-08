using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

/// <summary>Request body to update a recuperation. Identity fields (work order, component, customer, supplier) and status are ignored if sent.</summary>
public record UpdateRecuperationResource(
    [Required(ErrorMessage = "RequiredManufacturingOrderNumber")] [MaxLength(30)] string ManufacturingOrderNumber,
    [MaxLength(40)] string? Segment,
    [Required(ErrorMessage = "RequiredOperation")] [MaxLength(120)] string Operation,
    [Range(0.001, 100000, ErrorMessage = "InvalidWeight")] decimal WeightValue,
    [MaxLength(10)] string? WeightUnitSymbol,
    [Range(0, int.MaxValue, ErrorMessage = "InvalidHourmeter")] int HourmeterAtEntry,
    [Required(ErrorMessage = "RequiredPowderSupplier")] [MaxLength(80)] string PowderSupplier,
    [Required(ErrorMessage = "RequiredPowderLotNumber")] [MaxLength(40)] string PowderLotNumber,
    [Required(ErrorMessage = "RequiredPowderChemicalComposition")] [MaxLength(40)] string PowderChemicalComposition);
