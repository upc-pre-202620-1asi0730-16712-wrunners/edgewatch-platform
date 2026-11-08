using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

/// <summary>Request body to open a recuperation. Segment, weightUnitSymbol and receivedAt default to "Mining", "kg" and today.</summary>
public record OpenRecuperationResource(
    [Required(ErrorMessage = "RequiredSupplierOrganizationId")] [Range(1, int.MaxValue, ErrorMessage = "InvalidOrganizationId")] int SupplierOrganizationId,
    [Required(ErrorMessage = "RequiredWorkOrderNumber")] [RegularExpression("^[A-Z0-9-]{4,30}$", ErrorMessage = "InvalidWorkOrderNumber")] string WorkOrderNumber,
    [Required(ErrorMessage = "RequiredManufacturingOrderNumber")] [MaxLength(30)] string ManufacturingOrderNumber,
    [Range(1, int.MaxValue, ErrorMessage = "InvalidComponentId")] int ComponentId,
    [Range(1, int.MaxValue, ErrorMessage = "InvalidCustomerId")] int CustomerId,
    [MaxLength(40)] string? Segment,
    [Required(ErrorMessage = "RequiredOperation")] [MaxLength(120)] string Operation,
    [Range(0.001, 100000, ErrorMessage = "InvalidWeight")] decimal WeightValue,
    [MaxLength(10)] string? WeightUnitSymbol,
    [Range(0, int.MaxValue, ErrorMessage = "InvalidHourmeter")] int HourmeterAtEntry,
    [Required(ErrorMessage = "RequiredPowderSupplier")] [MaxLength(80)] string PowderSupplier,
    [Required(ErrorMessage = "RequiredPowderLotNumber")] [MaxLength(40)] string PowderLotNumber,
    [Required(ErrorMessage = "RequiredPowderChemicalComposition")] [MaxLength(40)] string PowderChemicalComposition,
    DateOnly? ReceivedAt);
