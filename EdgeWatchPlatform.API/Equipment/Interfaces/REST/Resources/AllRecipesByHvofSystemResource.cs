using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

/// <summary>Request body to open a recuperation. Segment, weightUnitSymbol and receivedAt default to "Mining", "kg" and today.</summary>
public record AllRecipesByHvofSystemResource(
    [Required(ErrorMessage = "RequiredSupplierOrganizationId")]
    [Range(1, int.MaxValue, ErrorMessage = "InvalidHvofSystemId")]
    int HvofSystemId
);