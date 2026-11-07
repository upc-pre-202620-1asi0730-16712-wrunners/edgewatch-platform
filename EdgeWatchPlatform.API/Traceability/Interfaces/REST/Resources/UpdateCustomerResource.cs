using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

/// <summary>Request body to update a customer. The RUC is not editable and is ignored if sent.</summary>
public record UpdateCustomerResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidOrganizationId")] int? LinkedAssetOwnerOrganizationId,
    [Required(ErrorMessage = "RequiredLegalName")] [MaxLength(150)] string LegalName,
    [Required(ErrorMessage = "RequiredMineSite")] [MaxLength(120)] string MineSite);
