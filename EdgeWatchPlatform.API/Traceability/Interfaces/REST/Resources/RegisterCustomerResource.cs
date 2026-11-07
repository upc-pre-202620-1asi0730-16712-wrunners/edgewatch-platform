using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

/// <summary>Request body to register a customer.</summary>
public record RegisterCustomerResource(
    [Required(ErrorMessage = "RequiredSupplierOrganizationId")] [Range(1, int.MaxValue, ErrorMessage = "InvalidOrganizationId")] int SupplierOrganizationId,
    [Range(1, int.MaxValue, ErrorMessage = "InvalidOrganizationId")] int? LinkedAssetOwnerOrganizationId,
    [Required(ErrorMessage = "RequiredLegalName")] [MaxLength(150)] string LegalName,
    [Required(ErrorMessage = "RequiredRuc")] [RegularExpression(@"^\d{11}$", ErrorMessage = "InvalidRuc")] string Ruc,
    [Required(ErrorMessage = "RequiredMineSite")] [MaxLength(120)] string MineSite);
