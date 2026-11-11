using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

/// <summary>Request body to update an HVOF system (the code and the organization never change).</summary>
public record UpdateHvofSystemResource(
    [Required(ErrorMessage = "RequiredSerialNumber")] [MaxLength(60)] string SerialNumber,
    [Required(ErrorMessage = "RequiredSystemManufacturer")] [MaxLength(80)] string SystemManufacturer,
    [Required(ErrorMessage = "RequiredSystemModel")] [MaxLength(80)] string SystemModel,
    [Required(ErrorMessage = "RequiredFuelType")] [RegularExpression("^(HYDROGEN|KEROSENE|NATURAL_GAS|PROPANE)$", ErrorMessage = "InvalidFuelType")] string FuelType,
    [Required(ErrorMessage = "RequiredStatus")] [RegularExpression("^(ACTIVE|MAINTENANCE|OUT_OF_SERVICE)$", ErrorMessage = "InvalidHvofSystemStatus")] string Status);
