using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

/// <summary>Request body to register an HVOF system.</summary>
public record RegisterHvofSystemResource(
    [Required(ErrorMessage = "RequiredOrganizationId")] [Range(1, int.MaxValue, ErrorMessage = "InvalidOrganizationId")] int OrganizationId,
    [Required(ErrorMessage = "RequiredCode")] [RegularExpression("^[A-Z0-9-]{3,20}$", ErrorMessage = "InvalidCode")] string Code,
    [Required(ErrorMessage = "RequiredSerialNumber")] [MaxLength(60)] string SerialNumber,
    [Required(ErrorMessage = "RequiredSystemManufacturer")] [MaxLength(80)] string SystemManufacturer,
    [Required(ErrorMessage = "RequiredSystemModel")] [MaxLength(80)] string SystemModel,
    [Required(ErrorMessage = "RequiredFuelType")] [RegularExpression("^(HYDROGEN|KEROSENE|NATURAL_GAS|PROPANE)$", ErrorMessage = "InvalidFuelType")] string FuelType);
