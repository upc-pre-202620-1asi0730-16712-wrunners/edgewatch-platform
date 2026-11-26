using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

/// <summary>Request body to get all HVOF systems by organization. Segment, weightUnitSymbol and receivedAt default to "Mining", "kg" and today.</summary>
public record HvofSystemsByOrganizationResource(
    [Required(ErrorMessage = "RequiredOrganizationId")]
    [Range(1, int.MaxValue, ErrorMessage = "InvalidOrganizationId")]
    int OrganizationId
);