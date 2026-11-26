using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Iam.Interfaces.REST.Resources;

/// <summary>Request body to list users by organization. Segment, weightUnitSymbol and receivedAt default to "Mining", "kg" and today.</summary>
public record UsersByOrganizationResource(
    [Required(ErrorMessage = "RequiredSupplierOrganizationId")]
    [Range(1, int.MaxValue, ErrorMessage = "InvalidOrganizationId")]
    int OrganizationId
);